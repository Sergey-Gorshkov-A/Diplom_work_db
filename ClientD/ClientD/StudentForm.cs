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
    public partial class StudentForm : Form
    {
        private int studentId;
        private int userId;

        public StudentForm(int studentId, int userId)
        {
            this.studentId = studentId;
            this.userId = userId;
            InitializeComponent();
            LoadData();
        }

        private async void LoadData()
        {
            string response = await SendRequest("GetStudentMarksAndRating", studentId.ToString());
            var data = JsonConvert.DeserializeObject<dynamic>(response);

            string marksJson = data.MarksBySubject.ToString();
            List<MarksBySubject> marksBySubject = new List<MarksBySubject>();
            if (!string.IsNullOrEmpty(marksJson) && marksJson != "null")
            {
                marksBySubject = JsonConvert.DeserializeObject<List<MarksBySubject>>(marksJson);
            }

            int maxMarks = marksBySubject.Any() ? marksBySubject.Max(m => m.Marks.Count) : 0;

            dgvMarks.Columns.Clear();
            dgvMarks.Columns.Add("Subject", "Предмет");
            for (int i = 0; i < maxMarks; i++)
                dgvMarks.Columns.Add($"Mark{i + 1}", $"Оценка {i + 1}");

            foreach (var subj in marksBySubject)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgvMarks);
                row.Cells[0].Value = subj.Subject;
                for (int i = 0; i < subj.Marks.Count; i++)
                    row.Cells[i + 1].Value = subj.Marks[i];
                dgvMarks.Rows.Add(row);
            }

            int ratingPoints = data.RatingPoints;
            int ratingPlace = data.RatingPlace;
            lblRating.Text = $"Рейтинг: {ratingPoints} очков (место {ratingPlace})";

            string reviewsJson = data.Reviews.ToString();
            if (!string.IsNullOrEmpty(reviewsJson) && reviewsJson != "null")
            {
                var reviews = JsonConvert.DeserializeObject<List<dynamic>>(reviewsJson);
                lstReviews.Items.Clear();
                foreach (var rev in reviews)
                {
                    string sign = rev.Points == 1 ? "+" : "-";
                    lstReviews.Items.Add($"{rev.Date}: {rev.Text} [{sign}] от {rev.Author}");
                }
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
}
public class MarksBySubject { public string Subject { get; set; } public List<int> Marks { get; set; } }

