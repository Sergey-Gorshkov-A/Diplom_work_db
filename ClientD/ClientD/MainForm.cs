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
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ClientD
{
    public partial class MainForm : Form
    {
        private int teacherId;
        private int userId;
        private List<Subject> subjects;
        private List<Logbook> currentLogbooks;
        private List<Student> currentStudents;
        private List<LessonInfo> currentLessons;
        private Dictionary<(int studentId, int lessonId), int> eventsDict;
        private Dictionary<(int studentId, int lessonId), string> eventsPresence;
        private Dictionary<(int studentId, int lessonId), int> marks;
        private int currentLogbookId;

        public MainForm(int teacherId, int userId)
        {
            this.teacherId = teacherId;
            this.userId = userId;
            this.Resize += new System.EventHandler(this.Form_Resize);
            InitializeComponent();
            LoadSubjects();
        }

        private async void LoadSubjects()
        {
            try
            {
                var response = await SendRequest("GetSubjects", null);
                subjects = JsonConvert.DeserializeObject<List<Subject>>(response);
                listBoxSubjects.DataSource = subjects;
                listBoxSubjects.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки предметов: {ex.Message}");
            }
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            dataGridView1.Width = this.ClientSize.Width - 200;
            dataGridView1.Height = this.ClientSize.Height - 100;
        }

        private async void listBoxSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxSubjects.SelectedItem is Subject subject)
            {
                try
                {
                    var response = await SendRequest("GetLogbooksBySubject", subject.Id.ToString());
                    currentLogbooks = JsonConvert.DeserializeObject<List<Logbook>>(response);
                    listBoxLogbooks.DataSource = currentLogbooks;
                    listBoxLogbooks.DisplayMember = "GroupName";
                    dataGridView1.DataSource = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки журналов: {ex.Message}");
                }
            }
        }

        private (int, int) ParseKey(string key)
        {
            var trimmed = key.Trim('(', ')');
            var parts = trimmed.Split(',');
            if (parts.Length == 2 &&
                int.TryParse(parts[0].Trim(), out int sid) &&
                int.TryParse(parts[1].Trim(), out int lid))
                return (sid, lid);
            return (0, 0);
        }

        private async void listBoxLogbooks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxLogbooks.SelectedItem is Logbook logbook)
            {
                try
                {
                    currentLogbookId = logbook.Id;
                    
                    var request = new { LogbookId = logbook.Id };
                    var response = await SendRequest("GetJournalData", JsonConvert.SerializeObject(request));
                    var data = JsonConvert.DeserializeObject<dynamic>(response);
                    string marksJson = data.marks.ToString();

                    currentStudents = JsonConvert.DeserializeObject<List<Student>>(data.students.ToString());
                    currentLessons = JsonConvert.DeserializeObject<List<LessonInfo>>(data.lessons.ToString());

                    var eventsPresenceRaw = JsonConvert.DeserializeObject<Dictionary<string, string>>(data.eventsPresence.ToString());
                    eventsPresence = new Dictionary<(int, int), string>();
                    foreach (var kv in eventsPresenceRaw)
                    {
                        var parsed = ParseKey(kv.Key);
                        if (parsed.Item1 != 0)
                            eventsPresence[(parsed.Item1, parsed.Item2)] = kv.Value;
                    }

                    var eventsMarksRaw = JsonConvert.DeserializeObject<Dictionary<string, int>>(data.marks.ToString());
                    marks = new Dictionary<(int, int), int>();
                    if (eventsMarksRaw == null) eventsMarksRaw = new Dictionary<string, int>();
                    foreach (var kv in eventsMarksRaw)
                    {
                        var parsed = ParseKey(kv.Key);
                        if (parsed.Item1 != 0)
                            marks[(parsed.Item1, parsed.Item2)] = kv.Value;
                    }

                    BuildJournalTable();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки журнала: {ex.Message}");
                }
            }
        }

        private double ComputeAverageMark(int studentId)
        {
            var studentMarks = marks
                .Where(kvp => kvp.Key.studentId == studentId && kvp.Value > 0)
                .Select(kvp => kvp.Value)
                .ToList();

            if (studentMarks.Count == 0) return 0;
            return studentMarks.Average();
        }

        private void BuildJournalTable()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("StudentName", "Ученик");
            dataGridView1.Columns["StudentName"].Width = 150;

            dataGridView1.Columns.Add("AverageMark", "Ср. балл");
            dataGridView1.Columns["AverageMark"].Width = 60;
            dataGridView1.Columns["AverageMark"].ReadOnly = true;
            dataGridView1.Columns["AverageMark"].DefaultCellStyle.BackColor = Color.LightYellow;
            dataGridView1.Columns["AverageMark"].DefaultCellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);

            foreach (var lesson in currentLessons)
            {
                string date = lesson.Date;
                DateTime date_1 = DateTime.Parse(date);
                string date_f = date_1.ToString("dd.MM");
                dataGridView1.Columns.Add($"presence_{lesson.Id}", $"{date_f}\nПосещ.");
                dataGridView1.Columns[$"presence_{lesson.Id}"].Width = 50;
                dataGridView1.Columns[$"presence_{lesson.Id}"].DefaultCellStyle.BackColor = Color.LightGreen;
                dataGridView1.Columns.Add($"mark_{lesson.Id}", $"Оценка");
                dataGridView1.Columns[$"mark_{lesson.Id}"].Width = 50;
                dataGridView1.Columns[$"mark_{lesson.Id}"].DefaultCellStyle.BackColor = Color.LightCyan;
            }

            foreach (var student in currentStudents)
            {
                int rowIndex = dataGridView1.Rows.Add();
                dataGridView1.Rows[rowIndex].Cells["StudentName"].Value = student.Fio;

                double average = ComputeAverageMark(student.Id);
                dataGridView1.Rows[rowIndex].Cells["AverageMark"].Value = average == 0 ? "" : average.ToString("F2");

                for (int i = 0; i < currentLessons.Count; i++)
                {
                    var lesson = currentLessons[i];
                    string presence = eventsPresence.ContainsKey((student.Id, lesson.Id)) ? eventsPresence[(student.Id, lesson.Id)] : "";
                    int mark = marks.ContainsKey((student.Id, lesson.Id)) ? marks[(student.Id, lesson.Id)] : 0;

                    DataGridViewComboBoxCell presenceCell = new DataGridViewComboBoxCell();
                    presenceCell.Items.AddRange(new string[] { "", "+", "Н", "П" });
                    presenceCell.Value = presence;
                    dataGridView1.Rows[rowIndex].Cells[$"presence_{lesson.Id}"] = presenceCell;

                    DataGridViewComboBoxCell markCell = new DataGridViewComboBoxCell();
                    markCell.Items.AddRange(new string[] { "", "2", "3", "4", "5" });
                    markCell.Value = mark == 0 ? "" : mark.ToString();
                    dataGridView1.Rows[rowIndex].Cells[$"mark_{lesson.Id}"] = markCell;
                }
            }

            //dataGridView1.Columns.Add("AverageMark", "Ср. балл");
            //dataGridView1.Columns["AverageMark"].Width = 60;
            //dataGridView1.Columns["AverageMark"].ReadOnly = true;
            //dataGridView1.Columns["AverageMark"].DefaultCellStyle.BackColor = Color.LightYellow;
            //dataGridView1.Columns["AverageMark"].DefaultCellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);

            //for (int i = 0; i < currentStudents.Count; i++)
            //{
                //var student = currentStudents[i];
                //double average = ComputeAverageMark(student.Id);
                //dataGridView1.Rows[i].Cells["AverageMark"].Value = average == 0 ? "" : average.ToString("F2");
            //}
            
            dataGridView1.CellValueChanged += DataGridViewJournal_CellValueChanged;
            dataGridView1.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dataGridView1.IsCurrentCellDirty)
                    dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
        }

        private async void DataGridViewJournal_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 2) return;
            var student = currentStudents[e.RowIndex];
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (!colName.StartsWith("presence_") && !colName.StartsWith("mark_")) return;
            string[] parts = colName.Split('_');
            if (!int.TryParse(parts[1], out int lessonId)) return;
            var lesson = currentLessons.Find(l => l.Id == lessonId);
            if (lesson == null) return;

            string newPresence = null;
            int? newMark = null;

            if (colName.StartsWith("presence_"))
            {
                var cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewComboBoxCell;
                string value = cell?.Value?.ToString();
                if (string.IsNullOrEmpty(value))
                    newPresence = "";
                else
                    newPresence = value;
            }
            else if (colName.StartsWith("mark_"))
            {
                string val = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                if (string.IsNullOrEmpty(val))
                    newMark = 0;
                else if (int.TryParse(val, out int mark))
                    newMark = mark;
                else
                    return;
            }

            var update = new CellUpdate
            {
                StudentId = student.Id,
                LessonId = lesson.Id,
                LogbookId = currentLogbookId,
                Presence = newPresence,
                Mark = newMark
            };

            try
            {
                await SendRequest("UpdateCell", JsonConvert.SerializeObject(update));

                if (newPresence != null)
                {
                    if (string.IsNullOrEmpty(newPresence))
                        eventsPresence.Remove((student.Id, lesson.Id));
                    else
                        eventsPresence[(student.Id, lesson.Id)] = newPresence;
                }

                if (newMark.HasValue)
                {
                    if (newMark.Value == 0)
                        marks.Remove((student.Id, lesson.Id));
                    else
                        marks[(student.Id, lesson.Id)] = newMark.Value;
                }

                double newAverage = ComputeAverageMark(student.Id);
                dataGridView1.Rows[e.RowIndex].Cells["AverageMark"].Value = newAverage == 0 ? "" : newAverage.ToString("F2");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления: {ex.Message}");
                await RefreshJournal();
            }
        }

        private async Task RefreshJournal()
        {
            if (currentLogbookId == 0) return;
            var request = new { LogbookId = currentLogbookId };
            var response = await SendRequest("GetJournalData", JsonConvert.SerializeObject(request));
            var data = JsonConvert.DeserializeObject<dynamic>(response);

            currentStudents = JsonConvert.DeserializeObject<List<Student>>(data.students.ToString());
            currentLessons = JsonConvert.DeserializeObject<List<LessonInfo>>(data.lessons.ToString());

            var eventsPresenceRaw = JsonConvert.DeserializeObject<Dictionary<string, string>>(data.eventsPresence.ToString());
            eventsPresence = new Dictionary<(int, int), string>();
            foreach (var kv in eventsPresenceRaw)
            {
                var parsed = ParseKey(kv.Key);
                if (parsed.studentId != 0)
                    eventsPresence[(parsed.studentId, parsed.lessonId)] = kv.Value;
            }

            var eventsMarksRaw = JsonConvert.DeserializeObject<Dictionary<string, int>>(data.marks.ToString());
            marks = new Dictionary<(int, int), int>();
            foreach (var kv in eventsMarksRaw)
            {
                var parsed = ParseKey(kv.Key);
                if (parsed.studentId != 0)
                    marks[(parsed.studentId, parsed.lessonId)] = kv.Value;
            }

            BuildJournalTable();
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

        private void buttonCalculateCost_Click(object sender, EventArgs e)
        {
            var calcForm = new CostCalculatorForm();
            calcForm.ShowDialog();
        }

        private void BtnAddReview_Click(object sender, EventArgs e)
        {
            var reviewForm = new ReviewForm(userId);
            reviewForm.ShowDialog();
        }

        private void BtnRating_Click(object sender, EventArgs e)
        {
            var ratingForm = new RatingForm();
            ratingForm.ShowDialog();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            dataGridView1.Width = this.ClientSize.Width - 200;
            dataGridView1.Height = this.ClientSize.Height - 100;
        }
    }

    public class Subject { public int Id { get; set; } public string Name { get; set; } }
    public class Logbook { public int Id { get; set; } public string GroupName { get; set; } }
    public class Student { public int Id { get; set; } public string Fio { get; set; } }
    public class LessonInfo { public int Id { get; set; } public string Date { get; set; } }

    public class CellUpdate
    {
        public int StudentId { get; set; }
        public int LessonId { get; set; }
        public int LogbookId { get; set; }
        public string Presence { get; set; }
        public int? Mark { get; set; }
    }
}