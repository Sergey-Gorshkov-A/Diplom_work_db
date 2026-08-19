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
    public partial class AdminForm : Form
    {
        private int userId;
        public AdminForm(int userId)
        {
            this.userId = userId;
            InitializeComponent();
            LoadUsers();
            LoadGroups();
            LoadSubjects();
            LoadSchedule();
            LoadStudents();
            LoadGroupsForCombo();
            LoadStudentUsers();
            LoadParent();
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
        // Вкладка "Пользователи"
        private async void LoadUsers()
        {
            try
            {
                string response = await SendRequest("GetUsers", null);
                var users = JsonConvert.DeserializeObject<List<dynamic>>(response);
                dgvUsers.DataSource = users;
                dgvUsers.Columns["Id"].ReadOnly = true;
                dgvUsers.Columns["Login"].ReadOnly = true;
                dgvUsers.Columns["RoleName"].ReadOnly = false;
                dgvUsers.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пользователей: {ex.Message}");
            }
        }

        private async void btnAddUser_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = cmbRole.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            var req = new { Login = login, Password = password, RoleName = role };
            try
            {
                await SendRequest("CreateUser", JsonConvert.SerializeObject(req));
                MessageBox.Show("Пользователь добавлен");
                LoadUsers();
                txtLogin.Clear();
                txtPassword.Clear();
                cmbRole.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private async void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0) return;
            var idValue = dgvUsers.SelectedRows[0].Cells["Id"].Value;
            if (idValue == null) return;
            int userIdDel = Convert.ToInt32(idValue);
            if (MessageBox.Show("Удалить пользователя?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    await SendRequest("DeleteUser", userIdDel.ToString());
                    LoadUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }

        private async void dgvUsers_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            var idCell = dgvUsers.Rows[e.RowIndex].Cells["Id"];
            if (idCell.Value == null) return;
            int userIdUpd = Convert.ToInt32(idCell.Value);
            var roleCell = dgvUsers.Rows[e.RowIndex].Cells["RoleName"];
            string newRole = roleCell.Value?.ToString();
            if (string.IsNullOrEmpty(newRole)) return;
            var req = new { UserId = userIdUpd, NewRoleName = newRole };
            try
            {
                await SendRequest("UpdateUserRole", JsonConvert.SerializeObject(req));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
                LoadUsers();
            }
        }

        private async void btnUpdateRole_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0) return;
            var idValue = dgvUsers.SelectedRows[0].Cells["Id"].Value;
            if (idValue == null) return;
            int userIdUp = Convert.ToInt32(idValue);
            string newRole = cmbRole.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(newRole)) return;
            var req = new { UserId = userIdUp, NewRoleName = newRole };
            try
            {
                await SendRequest("UpdateUserRole", JsonConvert.SerializeObject(req));
                MessageBox.Show("Роль обновлена");
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
        // Вкладка "Группы"
        private async void LoadGroups()
        {
            try
            {
                string response = await SendRequest("GetGroups", null);
                var groups = JsonConvert.DeserializeObject<List<dynamic>>(response);
                dgvGroups.DataSource = groups;
                dgvGroups.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки групп: {ex.Message}");
            }
        }

        private async void btnAddGroup_Click(object sender, EventArgs e)
        {
            string name = txtGroupName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите название группы");
                return;
            }
            try
            {
                await SendRequest("AddGroup", JsonConvert.SerializeObject(new { Name = name }));
                MessageBox.Show("Группа добавлена");
                LoadGroups();
                txtGroupName.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private async void btnDeleteGroup_Click(object sender, EventArgs e)
        {
            if (dgvGroups.SelectedRows.Count == 0) return;
            var idValue = dgvGroups.SelectedRows[0].Cells["Id"].Value;
            if (idValue == null) return;
            int groupId = Convert.ToInt32(idValue);
            if (MessageBox.Show("Удалить группу?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    await SendRequest("DeleteGroup", groupId.ToString());
                    LoadGroups();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }

        private async void btnUpdateGroup_Click(object sender, EventArgs e)
        {
            if (dgvGroups.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите группу для редактирования");
                return;
            }
            var idValue = dgvGroups.SelectedRows[0].Cells["Id"].Value;
            if (idValue == null) return;
            int groupId = Convert.ToInt32(idValue);
            string newName = txtEditGroupName.Text.Trim();
            if (string.IsNullOrEmpty(newName))
            {
                MessageBox.Show("Введите новое название группы");
                return;
            }
            var req = new { GroupId = groupId, NewName = newName };
            try
            {
                await SendRequest("UpdateGroup", JsonConvert.SerializeObject(req));
                MessageBox.Show("Название группы обновлено");
                LoadGroups();
                txtEditGroupName.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
        // Вкладка "Предметы"
        private async void LoadSubjects()
        {
            try
            {
                string response = await SendRequest("GetSubjects", null);
                var subjects = JsonConvert.DeserializeObject<List<dynamic>>(response);
                dgvSubjects.DataSource = subjects;
                dgvSubjects.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки предметов: {ex.Message}");
            }
        }

        private async void btnAddSubject_Click(object sender, EventArgs e)
        {
            string name = txtSubjectName.Text.Trim();
            if (!int.TryParse(txtSubjectPrice.Text, out int price)) price = 0;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите название предмета");
                return;
            }
            try
            {
                await SendRequest("AddSubject", JsonConvert.SerializeObject(new { Name = name, Price = price }));
                MessageBox.Show("Предмет добавлен");
                LoadSubjects();
                txtSubjectName.Clear();
                txtSubjectPrice.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private async void btnDeleteSubject_Click(object sender, EventArgs e)
        {
            if (dgvSubjects.SelectedRows.Count == 0) return;
            var idValue = dgvSubjects.SelectedRows[0].Cells["Id"].Value;
            if (idValue == null) return;
            int subjectId = Convert.ToInt32(idValue);
            if (MessageBox.Show("Удалить предмет?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    await SendRequest("DeleteSubject", subjectId.ToString());
                    LoadSubjects();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }

        private async void btnUpdateSubject_Click(object sender, EventArgs e)
        {
            if (dgvSubjects.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите предмет для редактирования");
                return;
            }
            var idValue = dgvSubjects.SelectedRows[0].Cells["Id"].Value;
            if (idValue == null) return;
            int subjectId = Convert.ToInt32(idValue);
            string newName = txtEditSubjectName.Text.Trim();
            if (string.IsNullOrEmpty(newName))
            {
                MessageBox.Show("Введите новое название предмета");
                return;
            }
            if (!int.TryParse(txtEditSubjectName.Text, out int newPrice))
            {
                MessageBox.Show("Введите корректную цену");
                return;
            }
            var req = new { SubjectId = subjectId, NewName = newName, NewPrice = newPrice };
            try
            {
                await SendRequest("UpdateSubject", JsonConvert.SerializeObject(req));
                MessageBox.Show("Предмет обновлён");
                LoadSubjects();
                txtEditSubjectName.Clear();
                txtEditSubjectName.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        // Вкладка "Расписание"
        private async void LoadSchedule()
        {
            try
            {
                string lessonsResponse = await SendRequest("GetLessons", null);
                var lessons = JsonConvert.DeserializeObject<List<dynamic>>(lessonsResponse);
                string daysResponse = await SendRequest("GetDays", null);
                var days = JsonConvert.DeserializeObject<List<dynamic>>(daysResponse);
                var dayDict = days.ToDictionary(d => (int)d.Id, d => $"{d.Year}-{d.Month}-{d.DayNumber}");

                var scheduleData = lessons.Select(l => {
                    int? dayId = l.DayId == null ? (int?)null : Convert.ToInt32(l.DayId);
                    string currentDay = dayId.HasValue && dayDict.ContainsKey(dayId.Value) ? dayDict[dayId.Value] : "Не назначен";
                    return new
                    {
                        Id = l.Id,
                        SubjectName = l.SubjectName,
                        CurrentDay = currentDay
                    };
                }).ToList();

                dgvSchedule.DataSource = scheduleData;
                dgvSchedule.AutoResizeColumns();

                var lessonsList = JsonConvert.DeserializeObject<List<dynamic>>(lessonsResponse);
                var lessonsWithIntId = lessonsList.Select(l => new { Id = Convert.ToInt32(l.Id), SubjectName = l.SubjectName }).ToList();
                cmbLesson.DataSource = lessonsWithIntId;
                cmbLesson.DisplayMember = "SubjectName";
                cmbLesson.ValueMember = "Id";

                var daysList = JsonConvert.DeserializeObject<List<dynamic>>(daysResponse);
                var daysWithIntId = daysList.Select(d => new
                {
                    Id = Convert.ToInt32(d.Id),
                    DateDisplay = $"{d.Year}-{d.Month}-{d.DayNumber}"
                }).ToList();
                cmbDay.DataSource = daysWithIntId;
                cmbDay.DisplayMember = "DateDisplay";
                cmbDay.ValueMember = "Id";

                var daysWithDisplay = daysList.Select(d => new { d.Id, DateDisplay = $"{d.Year}-{d.Month}-{d.DayNumber}" }).ToList();
                cmbDay.DataSource = daysWithDisplay;
                cmbDay.DisplayMember = "DateDisplay";
                cmbDay.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки расписания: {ex.Message}");
            }
        }

        private async void btnUpdateSchedule_Click(object sender, EventArgs e)
        {
            if (cmbLesson.SelectedValue == null || cmbDay.SelectedValue == null)
            {
                MessageBox.Show("Выберите урок и день");
                return;
            }
            int lessonId = Convert.ToInt32(cmbLesson.SelectedValue);
            int dayId = Convert.ToInt32(cmbDay.SelectedValue);
            try
            {
                await SendRequest("UpdateLessonDay", JsonConvert.SerializeObject(new { LessonId = lessonId, DayId = dayId }));
                MessageBox.Show("Расписание обновлено");
                LoadSchedule();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private async void btnDeleteSchedule_Click(object sender, EventArgs e)
        {
            if (dgvSchedule.SelectedRows.Count == 0) return;
            int lessonId = Convert.ToInt32(dgvSchedule.SelectedRows[0].Cells["Id"].Value);
            try
            {
                await SendRequest("RemoveLessonDay", lessonId.ToString());
                MessageBox.Show("Связь удалена");
                LoadSchedule();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        //Вкладка учащиеся
        private async void LoadStudentUsers()
        {
            try
            {
                string response = await SendRequest("GetUsersByRole", "student");
                var users = JsonConvert.DeserializeObject<List<dynamic>>(response);
                cmbStudentUser.DataSource = users;
                cmbStudentUser.DisplayMember = "Login";
                cmbStudentUser.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки учеников: {ex.Message}");
            }
        }

        private async void LoadParent()
        {
            try
            {
                string response = await SendRequest("GetParentsList", null);
                var parents = JsonConvert.DeserializeObject<List<dynamic>>(response);
                cmbParentUser.DataSource = parents;
                cmbParentUser.DisplayMember = "Fio";
                cmbParentUser.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки родителей: {ex.Message}");
            }
        }

        private async void LoadStudents()
        {
            try
            {
                string response = await SendRequest("GetStudentsWithGroups", null);
                var students = JsonConvert.DeserializeObject<List<dynamic>>(response);
                dgvStudents.DataSource = students;
                dgvStudents.Columns["Id"].ReadOnly = true;
                dgvStudents.Columns["Fio"].ReadOnly = true;
                dgvStudents.Columns["GroupName"].ReadOnly = true;
                dgvStudents.AutoResizeColumns();
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка загрузки учеников: {ex.Message}"); }
        }

        private async void LoadGroupsForCombo()
        {
            try
            {
                string response = await SendRequest("GetGroups", null);
                var groups = JsonConvert.DeserializeObject<List<dynamic>>(response);
                cmbStudentGroup.DataSource = groups;
                cmbStudentGroup.DisplayMember = "Name";
                cmbStudentGroup.ValueMember = "Id";
                cmbChangeGroup.DataSource = groups.ToList();
                cmbChangeGroup.DisplayMember = "Name";
                cmbChangeGroup.ValueMember = "Id";
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка загрузки групп: {ex.Message}"); }
        }

        private async void btnAddStudent_Click(object sender, EventArgs e)
        {
            string fio = txtStudentFio.Text.Trim();
            if (string.IsNullOrEmpty(fio) || cmbStudentGroup.SelectedValue == null ||
                cmbStudentUser.SelectedValue == null || cmbParentUser.SelectedValue == null)
            {
                MessageBox.Show("Заполните все поля (ФИО, группа, пользователь-ученик, родитель)");
                return;
            }
            int groupId = Convert.ToInt32(cmbStudentGroup.SelectedValue);
            int userId = Convert.ToInt32(cmbStudentUser.SelectedValue);
            int parentId = Convert.ToInt32(cmbParentUser.SelectedValue);
            var req = new { Fio = fio, GroupId = groupId, UserId = userId, ParentId = parentId };
            try
            {
                string response = await SendRequest("AddStudent", JsonConvert.SerializeObject(req));
                var result = JsonConvert.DeserializeObject<dynamic>(response);
                if (result.Success == true)
                {
                    MessageBox.Show("Ученик добавлен");
                    LoadStudents();
                    txtStudentFio.Clear();
                    cmbStudentGroup.SelectedIndex = -1;
                    cmbStudentUser.SelectedIndex = -1;
                    cmbParentUser.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show($"Ошибка: {result.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private async void btnDeleteStudent_Click(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count == 0) return;
            int studentId = Convert.ToInt32(dgvStudents.SelectedRows[0].Cells["Id"].Value);
            if (MessageBox.Show("Удалить ученика?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    await SendRequest("DeleteStudent", studentId.ToString());
                    LoadStudents();
                }
                catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}"); }
            }
        }

        private async void btnChangeGroup_Click(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count == 0) return;
            int studentId = Convert.ToInt32(dgvStudents.SelectedRows[0].Cells["Id"].Value);
            if (cmbChangeGroup.SelectedValue == null) return;
            int newGroupId = Convert.ToInt32(cmbChangeGroup.SelectedValue);
            var req = new { StudentId = studentId, NewGroupId = newGroupId };
            try
            {
                await SendRequest("UpdateStudentGroup", JsonConvert.SerializeObject(req));
                MessageBox.Show("Группа изменена");
                LoadStudents();
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}"); }
        }

        private async void btnViewProfile_Click(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count == 0) return;
            int studentId = Convert.ToInt32(dgvStudents.SelectedRows[0].Cells["Id"].Value);
            string response = await SendRequest("GetProfile", studentId.ToString());
            var profile = JsonConvert.DeserializeObject<Profile>(response);
            string msg = $"Фамилия: {profile.Surname}\nИмя: {profile.FirstName}\nОтчество: {profile.Patronymic}\nТелефон: {profile.Phone}\nАдрес: {profile.Address}\nЛичная информация: {profile.PersonalInfo}";
            MessageBox.Show(msg, "Профиль ученика");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
