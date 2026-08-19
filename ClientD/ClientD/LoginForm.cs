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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text.Trim();
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль");
                return;
            }
            try
            {
                var request = new { Login = login, Password = password };
                string response = await SendRequest("Login", JsonConvert.SerializeObject(request));
                var result = JsonConvert.DeserializeObject<dynamic>(response);
                if (result.Success == true)
                {
                    string role = result.Role.ToString();
                    int userId = result.UserId;
                    switch (role)
                    {
                        case "student":
                            int studentId = result.Extra.StudentId;
                            var studentForm = new StudentForm(studentId, userId);
                            studentForm.Show();
                            break;
                        case "parent":
                            int parentId = result.Extra.ParentId;
                            var parentForm = new ParentForm(parentId, userId);
                            parentForm.Show();
                            break;
                        case "teacher":
                            int teacherId = result.Extra.TeacherId;
                            var teacherForm = new MainForm(teacherId, userId);
                            teacherForm.Show();
                            break;
                        case "admin":
                            var adminForm = new AdminForm(userId);
                            adminForm.Show();
                            break;
                    }
                    this.Hide();
                }
                else
                {
                    MessageBox.Show(result.Message.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task<string> SendRequest(string action, string data)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка со стороны сервера: {ex.Message}");
                return null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
