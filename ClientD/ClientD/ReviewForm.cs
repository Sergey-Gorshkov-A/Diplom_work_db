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
    public partial class ReviewForm : Form
    {
        private int userId;

        public ReviewForm(int userId)
        {
            this.userId = userId;
            InitializeComponent();
            LoadStudents();
        }

        private async void LoadStudents()
        {
            string response = await SendRequest("GetStudents", null);
            var students = JsonConvert.DeserializeObject<List<Student>>(response);
            cmbStudents.DataSource = students;
            cmbStudents.DisplayMember = "Fio";
            cmbStudents.ValueMember = "Id";
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            string text = txtReview.Text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("Введите текст отзыва");
                return;
            }
            var review = new AddReviewRequest
            {
                Date = DateTime.Now,
                Text = txtReview.Text,
                IsPositive = chkPositive.Checked,
                UserId = userId,
                StudentId = (int)cmbStudents.SelectedValue
            };
            await SendRequest("AddReview", JsonConvert.SerializeObject(review));
            MessageBox.Show("Отзыв добавлен");
            Close();
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
public class AddReviewRequest { public DateTime Date { get; set; } public string Text { get; set; } public bool IsPositive { get; set; } public int UserId { get; set; } public int StudentId { get; set; } }