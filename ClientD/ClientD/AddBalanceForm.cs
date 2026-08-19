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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientD
{
    public partial class AddBalanceForm : Form
    {
        private int parentId;
        public AddBalanceForm(int parentId)
        {
            this.parentId = parentId;
            InitializeComponent();
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

        private async void btnOk_Click(object sender, EventArgs e)
        {
            string amountStr = txtAmount.Text.Trim();
            if (string.IsNullOrEmpty(amountStr) || !Regex.IsMatch(amountStr, @"^[1-9][0-9]*$"))
            {
                MessageBox.Show("Введите положительное целое число (без ведущих нулей и посторонних символов)");
                return;
            }
            int amount = int.Parse(amountStr);
            try
            {
                string response = await SendRequest("AddBalance", JsonConvert.SerializeObject(new { ParentId = parentId, Amount = amount }));
                var result = JsonConvert.DeserializeObject<dynamic>(response);
                if (result.NewBalance != null)
                {
                    MessageBox.Show($"Баланс пополнен. Новый баланс: {result.NewBalance} руб.");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Ошибка пополнения");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
