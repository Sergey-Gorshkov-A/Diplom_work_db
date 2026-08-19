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
    public partial class RatingForm : Form
    {
        public RatingForm()
        {
            InitializeComponent();
            LoadRating();
        }

        private async void LoadRating()
        {
            string response = await SendRequest("GetTeacherRating", null);
            var students = JsonConvert.DeserializeObject<List<dynamic>>(response);
            dgvRating.DataSource = students;
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

        private void RatingForm_Load(object sender, EventArgs e)
        {

        }
    }
}
