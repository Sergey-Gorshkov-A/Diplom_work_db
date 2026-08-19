using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientD
{
    public partial class CostCalculatorForm : Form
    {
        private List<Student> students;
        private List<Subject> subjects;

        public CostCalculatorForm()
        {
            InitializeComponent();
            LoadStudents();
        }

        private async void LoadStudents()
        {
            try
            {
                var studentsResponse = await SendRequest("GetStudents", null);
                students = JsonConvert.DeserializeObject<List<Student>>(studentsResponse);
                cmbStudents.DataSource = students;
                cmbStudents.DisplayMember = "Fio";
                cmbStudents.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            if (cmbStudents.SelectedItem == null)
            {
                MessageBox.Show("Выберите ученика");
                return;
            }

            int studentId = (int)cmbStudents.SelectedValue;
            var student = (Student)cmbStudents.SelectedItem;

            try
            {
                var response = await SendRequest("GetStudentCostsBySubject", studentId.ToString());
                var costs = JsonConvert.DeserializeObject<List<SubjectCost>>(response);

                if (costs == null || costs.Count == 0)
                {
                    MessageBox.Show("Для данного ученика нет учтённых уроков (посещения или неявки)");
                    return;
                }

                dgvReceipt.Rows.Clear();
                dgvReceipt.Columns.Clear();
                dgvReceipt.Columns.Add("Subject", "Предмет");
                dgvReceipt.Columns.Add("LessonCount", "Кол-во неоплаченных уроков");
                dgvReceipt.Columns.Add("TotalCost", "Стоимость, руб.");

                int totalLessons = 0;
                int totalSum = 0;
                foreach (var item in costs)
                {
                    dgvReceipt.Rows.Add(item.SubjectName, item.LessonCount, item.TotalCost);
                    totalLessons += item.LessonCount;
                    totalSum += item.TotalCost;
                }
                dgvReceipt.Rows.Add("ИТОГО", totalLessons, totalSum);

                dgvReceipt.Tag = student.Fio;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка расчёта: {ex.Message}");
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvReceipt.Rows.Count == 0)
            {
                MessageBox.Show("Сначала сформируйте квитанцию");
                return;
            }

            PrintDocument pd = new PrintDocument();
            pd.PrintPage += (s, ev) =>
            {
                string studentName = dgvReceipt.Tag?.ToString() ?? "Ученик";
                Font titleFont = new Font("Arial", 16, FontStyle.Bold);
                Font headerFont = new Font("Arial", 12, FontStyle.Bold);
                Font cellFont = new Font("Arial", 10);
                float y = 50;
                float leftMargin = ev.MarginBounds.Left;
                float topMargin = ev.MarginBounds.Top;

                ev.Graphics.DrawString("Квитанция об оплате", titleFont, Brushes.Black, leftMargin, y);
                y += 30;
                ev.Graphics.DrawString($"Ученик: {studentName}", headerFont, Brushes.Black, leftMargin, y);
                y += 30;
                ev.Graphics.DrawString($"Дата: {DateTime.Now.ToShortDateString()}", headerFont, Brushes.Black, leftMargin, y);
                y += 40;

                float col1 = leftMargin;
                float col2 = leftMargin + 150;
                float col3 = leftMargin + 300;
                ev.Graphics.DrawString("Предмет", headerFont, Brushes.Black, col1, y);
                ev.Graphics.DrawString("Кол-во неоплаченных уроков", headerFont, Brushes.Black, col2, y);
                ev.Graphics.DrawString("Стоимость, руб.", headerFont, Brushes.Black, col3, y);
                y += 20;
                ev.Graphics.DrawLine(Pens.Black, leftMargin, y, leftMargin + 400, y);
                y += 10;

                for (int i = 0; i < dgvReceipt.Rows.Count; i++)
                {
                    var row = dgvReceipt.Rows[i];
                    string subject = row.Cells[0].Value?.ToString() ?? "";
                    string lessonCount = row.Cells[1].Value?.ToString() ?? "";
                    string cost = row.Cells[2].Value?.ToString() ?? "";
                    ev.Graphics.DrawString(subject, cellFont, Brushes.Black, col1, y);
                    ev.Graphics.DrawString(lessonCount, cellFont, Brushes.Black, col2, y);
                    ev.Graphics.DrawString(cost, cellFont, Brushes.Black, col3, y);
                    y += 20;
                }

                ev.HasMorePages = false;
            };

            PrintPreviewDialog ppd = new PrintPreviewDialog();
            ppd.Document = pd;
            ppd.ShowDialog();
        }

        private async void btnCalculate_Click(object sender, EventArgs e)
        {
            //if (listBoxStudents.SelectedItem == null || listBoxSubjects.SelectedItem == null)
            //{
                //MessageBox.Show("Выберите ученика и предмет");
                //return;
            //}

            //var student = (Student)listBoxStudents.SelectedItem;
            //var subject = (Subject)listBoxSubjects.SelectedItem;

            //var request = new CalculateRequest { StudentId = student.Id, SubjectId = subject.Id };
            //var response = await SendRequest("CalculateCost", JsonConvert.SerializeObject(request));
            //var result = JsonConvert.DeserializeObject<dynamic>(response);
            //int totalCost = result.TotalCost;
            //int lessonCount = result.LessonCount;

            //MessageBox.Show($"Стоимость уроков (посещённых и неявок) по предмету '{subject.Name}': {totalCost} руб.\nКоличество учтённых уроков: {lessonCount}");
        }

        private async Task<string> SendRequest(string action, string data)
        {
            using (var client = new TcpClient())
            {
                await client.ConnectAsync("127.0.0.1", 8888);
                using (var stream = client.GetStream())
                using (var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    var msg = new { Action = action, Data = data };
                    string json = JsonConvert.SerializeObject(msg);
                    await writer.WriteLineAsync(json);
                    string response = await reader.ReadLineAsync();
                    if (response == "error" || response == "unknown")
                        throw new Exception($"Сервер вернул: {response}");
                    return response;
                }
            }
        }

        private void listBoxSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

    public class CalculateRequest
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
    }

    public class SubjectCost
    {
        public string SubjectName { get; set; }
        public int LessonCount { get; set; }
        public int TotalCost { get; set; }
    }
}
