using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientD
{
    public partial class ParentForm : Form
    {
        private int parentId;
        private int userId;

        public ParentForm(int parentId, int userId)
        {
            this.parentId = parentId;
            this.userId = userId;
            InitializeComponent();
            this.Load += async (s, e) =>
            {
                await LoadChildren();
                await LoadBalance();
            };
        }

        private async Task LoadChildren()
        {
            try
            {
                string response = await SendRequest("GetChildren", parentId.ToString());
                var children = JsonConvert.DeserializeObject<List<Child>>(response);
                lstChildren.DataSource = children;
                lstChildren.DisplayMember = "Fio";
                lstChildren.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки детей: {ex.Message}");
            }
        }

        private async Task LoadBalance()
        {
            try
            {
                string response = await SendRequest("GetParentBalance", parentId.ToString());
                var data = JsonConvert.DeserializeObject<dynamic>(response);
                int balance = data.Balance;
                lblBalance.Text = $"Баланс: {balance} руб.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки баланса: {ex.Message}");
            }
        }

        private async void lstChildren_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstChildren.SelectedItem == null) return;
            var child = (Child)lstChildren.SelectedItem;
            int childId = child.Id;
            var request = new { StudentId = childId };
            try
            {
                string response = await SendRequest("GetChildData", JsonConvert.SerializeObject(request));
                var data = JsonConvert.DeserializeObject<dynamic>(response);

                string marksJson = data.MarksBySubject?.ToString();
                if (!string.IsNullOrEmpty(marksJson) && marksJson != "null")
                {
                    var marksBySubject = JsonConvert.DeserializeObject<List<MarksBySubject>>(marksJson);
                    BuildMarksTable(marksBySubject);
                }

                var attendance = JsonConvert.DeserializeObject<List<AttendanceRecord>>(data.Attendance.ToString());
                dgvAttendance.DataSource = attendance;

                var reviews = JsonConvert.DeserializeObject<List<dynamic>>(data.Reviews.ToString());
                lstReviews.Items.Clear();
                foreach (var rev in reviews)
                    lstReviews.Items.Add($"{rev.Date}: {rev.Text} [{(rev.Points == 1 ? "+" : "-")}]");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных ребёнка: {ex.Message}");
            }
        }

        private void BuildMarksTable(List<MarksBySubject> marksBySubject)
        {
            dgvChildMarks.Rows.Clear();
            dgvChildMarks.Columns.Clear();
            dgvChildMarks.Columns.Add("Subject", "Предмет");
            int maxMarks = marksBySubject.Any() ? marksBySubject.Max(m => m.Marks.Count) : 0;
            for (int i = 0; i < maxMarks; i++)
                dgvChildMarks.Columns.Add($"Mark{i + 1}", $"Оценка {i + 1}");
            foreach (var subj in marksBySubject)
            {
                var row = new DataGridViewRow();
                row.CreateCells(dgvChildMarks);
                row.Cells[0].Value = subj.Subject;
                for (int i = 0; i < subj.Marks.Count; i++)
                    row.Cells[i + 1].Value = subj.Marks[i];
                dgvChildMarks.Rows.Add(row);
            }
        }

        private void btnEditProfile_Click(object sender, EventArgs e)
        {
            if (lstChildren.SelectedItem == null) return;
            int childId = (int)lstChildren.SelectedValue;
            var profileForm = new ChildProfileForm(childId);
            profileForm.ShowDialog();
        }

        private async void btnAddBalance_Click(object sender, EventArgs e)
        {
            var form = new AddBalanceForm(parentId);
            if (form.ShowDialog() == DialogResult.OK)
            {
                await LoadBalance();
            }
        }

        private async void btnPayLessons_Click(object sender, EventArgs e)
        {
            try
            {
                string response = await SendRequest("PayAllPossibleLessons", parentId.ToString());
                var result = JsonConvert.DeserializeObject<dynamic>(response);
                int paidCount = result.PaidCount;
                int newBalance = result.NewBalance;
                MessageBox.Show($"Оплачено уроков: {paidCount}\nОстаток баланса: {newBalance} руб.");
                await LoadBalance();
                
                if (lstChildren.SelectedItem != null)
                    lstChildren_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка оплаты: {ex.Message}");
            }
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
                    return await reader.ReadLineAsync();
                }
            }
        }
    }
    public class Child
    {
        public int Id { get; set; }
        public string Fio { get; set; }
    }

    public class AttendanceRecord
    {
        public string Date { get; set; }
        public string Presence { get; set; }
    }
}
