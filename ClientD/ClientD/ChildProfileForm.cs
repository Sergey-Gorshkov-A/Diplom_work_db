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
using static System.Net.Mime.MediaTypeNames;

namespace ClientD
{
    public partial class ChildProfileForm : Form
    {
        private int studentId;

        public ChildProfileForm(int studentId)
        {
            this.studentId = studentId;
            InitializeComponent();
            LoadProfile();
        }

        private async void LoadProfile()
        {
            string response = await SendRequest("GetProfile", studentId.ToString());
            var profile = JsonConvert.DeserializeObject<Profile>(response);
            if (profile != null && profile.Id != 0)
            {
                txtSurname.Text = profile.Surname;
                txtFirstName.Text = profile.FirstName;
                txtPatronymic.Text = profile.Patronymic;
                txtPhone.Text = profile.Phone;
                txtAddress.Text = profile.Address;
                txtPersonalInfo.Text = profile.PersonalInfo;
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            string surname = txtSurname.Text.Trim();
            string firstname = txtFirstName.Text.Trim();
            if (string.IsNullOrEmpty(surname))
            {
                MessageBox.Show("Введите фамилию ученика");
                return;
            }
            if (string.IsNullOrEmpty(firstname))
            {
                MessageBox.Show("Введите имя ученика");
                return;
            }
            var profile = new Profile
            {
                StudentId = studentId,
                Surname = txtSurname.Text,
                FirstName = txtFirstName.Text,
                Patronymic = txtPatronymic.Text,
                Phone = txtPhone.Text,
                Address = txtAddress.Text,
                PersonalInfo = txtPersonalInfo.Text
            };
            await SendRequest("UpdateProfile", JsonConvert.SerializeObject(profile));
            MessageBox.Show("Профиль сохранён");
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

    public class Profile
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PersonalInfo { get; set; }
    }
}
