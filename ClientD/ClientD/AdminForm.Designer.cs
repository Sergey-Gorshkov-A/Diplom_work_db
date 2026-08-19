namespace ClientD
{
    partial class AdminForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabUsers = new System.Windows.Forms.TabPage();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.panelUsersTop = new System.Windows.Forms.Panel();
            this.btnUpdateRole = new System.Windows.Forms.Button();
            this.btnDeleteUser = new System.Windows.Forms.Button();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.tabGroups = new System.Windows.Forms.TabPage();
            this.dgvGroups = new System.Windows.Forms.DataGridView();
            this.panelGroupsTop = new System.Windows.Forms.Panel();
            this.btnUpdateGroup = new System.Windows.Forms.Button();
            this.txtEditGroupName = new System.Windows.Forms.TextBox();
            this.btnDeleteGroup = new System.Windows.Forms.Button();
            this.btnAddGroup = new System.Windows.Forms.Button();
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.tabSubjects = new System.Windows.Forms.TabPage();
            this.dgvSubjects = new System.Windows.Forms.DataGridView();
            this.panelSubjectsTop = new System.Windows.Forms.Panel();
            this.btnUpdateSubject = new System.Windows.Forms.Button();
            this.txtEditSubjectPrice = new System.Windows.Forms.TextBox();
            this.txtEditSubjectName = new System.Windows.Forms.TextBox();
            this.btnDeleteSubject = new System.Windows.Forms.Button();
            this.btnAddSubject = new System.Windows.Forms.Button();
            this.txtSubjectPrice = new System.Windows.Forms.TextBox();
            this.txtSubjectName = new System.Windows.Forms.TextBox();
            this.tabSchedule = new System.Windows.Forms.TabPage();
            this.dgvSchedule = new System.Windows.Forms.DataGridView();
            this.panelScheduleTop = new System.Windows.Forms.Panel();
            this.btnDeleteSchedule = new System.Windows.Forms.Button();
            this.btnUpdateSchedule = new System.Windows.Forms.Button();
            this.cmbDay = new System.Windows.Forms.ComboBox();
            this.cmbLesson = new System.Windows.Forms.ComboBox();
            this.tabStudents = new System.Windows.Forms.TabPage();
            this.dgvStudents = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblChangeGroup = new System.Windows.Forms.Label();
            this.cmbChangeGroup = new System.Windows.Forms.ComboBox();
            this.btnViewProfile = new System.Windows.Forms.Button();
            this.btnChangeGroup = new System.Windows.Forms.Button();
            this.btnDeleteStudent = new System.Windows.Forms.Button();
            this.btnAddStudent = new System.Windows.Forms.Button();
            this.cmbParentUser = new System.Windows.Forms.ComboBox();
            this.lblParentUser = new System.Windows.Forms.Label();
            this.cmbStudentUser = new System.Windows.Forms.ComboBox();
            this.lblStudentUser = new System.Windows.Forms.Label();
            this.cmbStudentGroup = new System.Windows.Forms.ComboBox();
            this.lblGroup = new System.Windows.Forms.Label();
            this.txtStudentFio = new System.Windows.Forms.TextBox();
            this.lblFio = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.panelUsersTop.SuspendLayout();
            this.tabGroups.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroups)).BeginInit();
            this.panelGroupsTop.SuspendLayout();
            this.tabSubjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubjects)).BeginInit();
            this.panelSubjectsTop.SuspendLayout();
            this.tabSchedule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSchedule)).BeginInit();
            this.panelScheduleTop.SuspendLayout();
            this.tabStudents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabUsers);
            this.tabControl1.Controls.Add(this.tabGroups);
            this.tabControl1.Controls.Add(this.tabSubjects);
            this.tabControl1.Controls.Add(this.tabSchedule);
            this.tabControl1.Controls.Add(this.tabStudents);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(882, 553);
            this.tabControl1.TabIndex = 0;
            // 
            // tabUsers
            // 
            this.tabUsers.Controls.Add(this.dgvUsers);
            this.tabUsers.Controls.Add(this.panelUsersTop);
            this.tabUsers.Location = new System.Drawing.Point(4, 25);
            this.tabUsers.Name = "tabUsers";
            this.tabUsers.Padding = new System.Windows.Forms.Padding(3);
            this.tabUsers.Size = new System.Drawing.Size(874, 524);
            this.tabUsers.TabIndex = 0;
            this.tabUsers.Text = "Пользователи";
            this.tabUsers.UseVisualStyleBackColor = true;
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUsers.Location = new System.Drawing.Point(3, 103);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.RowHeadersWidth = 51;
            this.dgvUsers.RowTemplate.Height = 24;
            this.dgvUsers.Size = new System.Drawing.Size(868, 418);
            this.dgvUsers.TabIndex = 1;
            this.dgvUsers.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUsers_CellEndEdit);
            // 
            // panelUsersTop
            // 
            this.panelUsersTop.Controls.Add(this.btnUpdateRole);
            this.panelUsersTop.Controls.Add(this.btnDeleteUser);
            this.panelUsersTop.Controls.Add(this.btnAddUser);
            this.panelUsersTop.Controls.Add(this.cmbRole);
            this.panelUsersTop.Controls.Add(this.txtPassword);
            this.panelUsersTop.Controls.Add(this.txtLogin);
            this.panelUsersTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUsersTop.Location = new System.Drawing.Point(3, 3);
            this.panelUsersTop.Name = "panelUsersTop";
            this.panelUsersTop.Size = new System.Drawing.Size(868, 100);
            this.panelUsersTop.TabIndex = 0;
            // 
            // btnUpdateRole
            // 
            this.btnUpdateRole.Location = new System.Drawing.Point(660, 10);
            this.btnUpdateRole.Name = "btnUpdateRole";
            this.btnUpdateRole.Size = new System.Drawing.Size(120, 23);
            this.btnUpdateRole.TabIndex = 5;
            this.btnUpdateRole.Text = "Обновить роль";
            this.btnUpdateRole.UseVisualStyleBackColor = true;
            this.btnUpdateRole.Click += new System.EventHandler(this.btnUpdateRole_Click);
            // 
            // btnDeleteUser
            // 
            this.btnDeleteUser.Location = new System.Drawing.Point(550, 10);
            this.btnDeleteUser.Name = "btnDeleteUser";
            this.btnDeleteUser.Size = new System.Drawing.Size(100, 23);
            this.btnDeleteUser.TabIndex = 4;
            this.btnDeleteUser.Text = "Удалить";
            this.btnDeleteUser.UseVisualStyleBackColor = true;
            this.btnDeleteUser.Click += new System.EventHandler(this.btnDeleteUser_Click);
            // 
            // btnAddUser
            // 
            this.btnAddUser.Location = new System.Drawing.Point(440, 10);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(100, 23);
            this.btnAddUser.TabIndex = 3;
            this.btnAddUser.Text = "Добавить";
            this.btnAddUser.UseVisualStyleBackColor = true;
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
            // 
            // cmbRole
            // 
            this.cmbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRole.FormattingEnabled = true;
            this.cmbRole.Items.AddRange(new object[] {
            "admin",
            "student",
            "parent",
            "teacher"});
            this.cmbRole.Location = new System.Drawing.Point(330, 10);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new System.Drawing.Size(100, 24);
            this.cmbRole.TabIndex = 2;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(170, 10);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(150, 22);
            this.txtPassword.TabIndex = 1;
            // 
            // txtLogin
            // 
            this.txtLogin.Location = new System.Drawing.Point(10, 10);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(150, 22);
            this.txtLogin.TabIndex = 0;
            // 
            // tabGroups
            // 
            this.tabGroups.Controls.Add(this.dgvGroups);
            this.tabGroups.Controls.Add(this.panelGroupsTop);
            this.tabGroups.Location = new System.Drawing.Point(4, 25);
            this.tabGroups.Name = "tabGroups";
            this.tabGroups.Padding = new System.Windows.Forms.Padding(3);
            this.tabGroups.Size = new System.Drawing.Size(874, 524);
            this.tabGroups.TabIndex = 1;
            this.tabGroups.Text = "Группы";
            this.tabGroups.UseVisualStyleBackColor = true;
            // 
            // dgvGroups
            // 
            this.dgvGroups.AllowUserToAddRows = false;
            this.dgvGroups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGroups.Location = new System.Drawing.Point(3, 43);
            this.dgvGroups.Name = "dgvGroups";
            this.dgvGroups.RowHeadersWidth = 51;
            this.dgvGroups.RowTemplate.Height = 24;
            this.dgvGroups.Size = new System.Drawing.Size(868, 478);
            this.dgvGroups.TabIndex = 1;
            // 
            // panelGroupsTop
            // 
            this.panelGroupsTop.Controls.Add(this.btnUpdateGroup);
            this.panelGroupsTop.Controls.Add(this.txtEditGroupName);
            this.panelGroupsTop.Controls.Add(this.btnDeleteGroup);
            this.panelGroupsTop.Controls.Add(this.btnAddGroup);
            this.panelGroupsTop.Controls.Add(this.txtGroupName);
            this.panelGroupsTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelGroupsTop.Location = new System.Drawing.Point(3, 3);
            this.panelGroupsTop.Name = "panelGroupsTop";
            this.panelGroupsTop.Size = new System.Drawing.Size(868, 40);
            this.panelGroupsTop.TabIndex = 0;
            // 
            // btnUpdateGroup
            // 
            this.btnUpdateGroup.Location = new System.Drawing.Point(542, 6);
            this.btnUpdateGroup.Name = "btnUpdateGroup";
            this.btnUpdateGroup.Size = new System.Drawing.Size(157, 23);
            this.btnUpdateGroup.TabIndex = 2;
            this.btnUpdateGroup.Text = "Редактировать";
            this.btnUpdateGroup.UseVisualStyleBackColor = true;
            this.btnUpdateGroup.Click += new System.EventHandler(this.btnUpdateGroup_Click);
            // 
            // txtEditGroupName
            // 
            this.txtEditGroupName.Location = new System.Drawing.Point(386, 6);
            this.txtEditGroupName.Name = "txtEditGroupName";
            this.txtEditGroupName.Size = new System.Drawing.Size(150, 22);
            this.txtEditGroupName.TabIndex = 3;
            // 
            // btnDeleteGroup
            // 
            this.btnDeleteGroup.Location = new System.Drawing.Point(280, 5);
            this.btnDeleteGroup.Name = "btnDeleteGroup";
            this.btnDeleteGroup.Size = new System.Drawing.Size(100, 23);
            this.btnDeleteGroup.TabIndex = 2;
            this.btnDeleteGroup.Text = "Удалить";
            this.btnDeleteGroup.UseVisualStyleBackColor = true;
            this.btnDeleteGroup.Click += new System.EventHandler(this.btnDeleteGroup_Click);
            // 
            // btnAddGroup
            // 
            this.btnAddGroup.Location = new System.Drawing.Point(170, 5);
            this.btnAddGroup.Name = "btnAddGroup";
            this.btnAddGroup.Size = new System.Drawing.Size(100, 23);
            this.btnAddGroup.TabIndex = 1;
            this.btnAddGroup.Text = "Добавить";
            this.btnAddGroup.UseVisualStyleBackColor = true;
            this.btnAddGroup.Click += new System.EventHandler(this.btnAddGroup_Click);
            // 
            // txtGroupName
            // 
            this.txtGroupName.Location = new System.Drawing.Point(10, 5);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(150, 22);
            this.txtGroupName.TabIndex = 0;
            // 
            // tabSubjects
            // 
            this.tabSubjects.Controls.Add(this.dgvSubjects);
            this.tabSubjects.Controls.Add(this.panelSubjectsTop);
            this.tabSubjects.Location = new System.Drawing.Point(4, 25);
            this.tabSubjects.Name = "tabSubjects";
            this.tabSubjects.Size = new System.Drawing.Size(874, 524);
            this.tabSubjects.TabIndex = 2;
            this.tabSubjects.Text = "Предметы";
            this.tabSubjects.UseVisualStyleBackColor = true;
            // 
            // dgvSubjects
            // 
            this.dgvSubjects.AllowUserToAddRows = false;
            this.dgvSubjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSubjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSubjects.Location = new System.Drawing.Point(0, 40);
            this.dgvSubjects.Name = "dgvSubjects";
            this.dgvSubjects.RowHeadersWidth = 51;
            this.dgvSubjects.RowTemplate.Height = 24;
            this.dgvSubjects.Size = new System.Drawing.Size(874, 484);
            this.dgvSubjects.TabIndex = 1;
            // 
            // panelSubjectsTop
            // 
            this.panelSubjectsTop.Controls.Add(this.btnUpdateSubject);
            this.panelSubjectsTop.Controls.Add(this.txtEditSubjectPrice);
            this.panelSubjectsTop.Controls.Add(this.txtEditSubjectName);
            this.panelSubjectsTop.Controls.Add(this.btnDeleteSubject);
            this.panelSubjectsTop.Controls.Add(this.btnAddSubject);
            this.panelSubjectsTop.Controls.Add(this.txtSubjectPrice);
            this.panelSubjectsTop.Controls.Add(this.txtSubjectName);
            this.panelSubjectsTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSubjectsTop.Location = new System.Drawing.Point(0, 0);
            this.panelSubjectsTop.Name = "panelSubjectsTop";
            this.panelSubjectsTop.Size = new System.Drawing.Size(874, 40);
            this.panelSubjectsTop.TabIndex = 0;
            // 
            // btnUpdateSubject
            // 
            this.btnUpdateSubject.Location = new System.Drawing.Point(758, 5);
            this.btnUpdateSubject.Name = "btnUpdateSubject";
            this.btnUpdateSubject.Size = new System.Drawing.Size(108, 23);
            this.btnUpdateSubject.TabIndex = 2;
            this.btnUpdateSubject.Text = "Изменить";
            this.btnUpdateSubject.UseVisualStyleBackColor = true;
            this.btnUpdateSubject.Click += new System.EventHandler(this.btnUpdateSubject_Click);
            // 
            // txtEditSubjectPrice
            // 
            this.txtEditSubjectPrice.Location = new System.Drawing.Point(652, 5);
            this.txtEditSubjectPrice.Name = "txtEditSubjectPrice";
            this.txtEditSubjectPrice.Size = new System.Drawing.Size(100, 22);
            this.txtEditSubjectPrice.TabIndex = 2;
            // 
            // txtEditSubjectName
            // 
            this.txtEditSubjectName.Location = new System.Drawing.Point(496, 5);
            this.txtEditSubjectName.Name = "txtEditSubjectName";
            this.txtEditSubjectName.Size = new System.Drawing.Size(150, 22);
            this.txtEditSubjectName.TabIndex = 2;
            // 
            // btnDeleteSubject
            // 
            this.btnDeleteSubject.Location = new System.Drawing.Point(390, 5);
            this.btnDeleteSubject.Name = "btnDeleteSubject";
            this.btnDeleteSubject.Size = new System.Drawing.Size(100, 23);
            this.btnDeleteSubject.TabIndex = 3;
            this.btnDeleteSubject.Text = "Удалить";
            this.btnDeleteSubject.UseVisualStyleBackColor = true;
            this.btnDeleteSubject.Click += new System.EventHandler(this.btnDeleteSubject_Click);
            // 
            // btnAddSubject
            // 
            this.btnAddSubject.Location = new System.Drawing.Point(280, 5);
            this.btnAddSubject.Name = "btnAddSubject";
            this.btnAddSubject.Size = new System.Drawing.Size(100, 23);
            this.btnAddSubject.TabIndex = 2;
            this.btnAddSubject.Text = "Добавить";
            this.btnAddSubject.UseVisualStyleBackColor = true;
            this.btnAddSubject.Click += new System.EventHandler(this.btnAddSubject_Click);
            // 
            // txtSubjectPrice
            // 
            this.txtSubjectPrice.Location = new System.Drawing.Point(170, 5);
            this.txtSubjectPrice.Name = "txtSubjectPrice";
            this.txtSubjectPrice.Size = new System.Drawing.Size(100, 22);
            this.txtSubjectPrice.TabIndex = 1;
            // 
            // txtSubjectName
            // 
            this.txtSubjectName.Location = new System.Drawing.Point(10, 5);
            this.txtSubjectName.Name = "txtSubjectName";
            this.txtSubjectName.Size = new System.Drawing.Size(150, 22);
            this.txtSubjectName.TabIndex = 0;
            // 
            // tabSchedule
            // 
            this.tabSchedule.Controls.Add(this.dgvSchedule);
            this.tabSchedule.Controls.Add(this.panelScheduleTop);
            this.tabSchedule.Location = new System.Drawing.Point(4, 25);
            this.tabSchedule.Name = "tabSchedule";
            this.tabSchedule.Size = new System.Drawing.Size(874, 524);
            this.tabSchedule.TabIndex = 3;
            this.tabSchedule.Text = "Расписание";
            this.tabSchedule.UseVisualStyleBackColor = true;
            // 
            // dgvSchedule
            // 
            this.dgvSchedule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSchedule.Location = new System.Drawing.Point(0, 50);
            this.dgvSchedule.Name = "dgvSchedule";
            this.dgvSchedule.RowHeadersWidth = 51;
            this.dgvSchedule.RowTemplate.Height = 24;
            this.dgvSchedule.Size = new System.Drawing.Size(874, 474);
            this.dgvSchedule.TabIndex = 1;
            // 
            // panelScheduleTop
            // 
            this.panelScheduleTop.Controls.Add(this.btnDeleteSchedule);
            this.panelScheduleTop.Controls.Add(this.btnUpdateSchedule);
            this.panelScheduleTop.Controls.Add(this.cmbDay);
            this.panelScheduleTop.Controls.Add(this.cmbLesson);
            this.panelScheduleTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelScheduleTop.Location = new System.Drawing.Point(0, 0);
            this.panelScheduleTop.Name = "panelScheduleTop";
            this.panelScheduleTop.Size = new System.Drawing.Size(874, 50);
            this.panelScheduleTop.TabIndex = 0;
            // 
            // btnDeleteSchedule
            // 
            this.btnDeleteSchedule.Location = new System.Drawing.Point(490, 10);
            this.btnDeleteSchedule.Name = "btnDeleteSchedule";
            this.btnDeleteSchedule.Size = new System.Drawing.Size(180, 23);
            this.btnDeleteSchedule.TabIndex = 3;
            this.btnDeleteSchedule.Text = "Отменить день";
            this.btnDeleteSchedule.UseVisualStyleBackColor = true;
            this.btnDeleteSchedule.Click += new System.EventHandler(this.btnDeleteSchedule_Click);
            // 
            // btnUpdateSchedule
            // 
            this.btnUpdateSchedule.Location = new System.Drawing.Point(330, 10);
            this.btnUpdateSchedule.Name = "btnUpdateSchedule";
            this.btnUpdateSchedule.Size = new System.Drawing.Size(150, 23);
            this.btnUpdateSchedule.TabIndex = 2;
            this.btnUpdateSchedule.Text = "Назначить день";
            this.btnUpdateSchedule.UseVisualStyleBackColor = true;
            this.btnUpdateSchedule.Click += new System.EventHandler(this.btnUpdateSchedule_Click);
            // 
            // cmbDay
            // 
            this.cmbDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDay.FormattingEnabled = true;
            this.cmbDay.Location = new System.Drawing.Point(170, 10);
            this.cmbDay.Name = "cmbDay";
            this.cmbDay.Size = new System.Drawing.Size(150, 24);
            this.cmbDay.TabIndex = 1;
            // 
            // cmbLesson
            // 
            this.cmbLesson.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLesson.FormattingEnabled = true;
            this.cmbLesson.Location = new System.Drawing.Point(10, 10);
            this.cmbLesson.Name = "cmbLesson";
            this.cmbLesson.Size = new System.Drawing.Size(150, 24);
            this.cmbLesson.TabIndex = 0;
            // 
            // tabStudents
            // 
            this.tabStudents.Controls.Add(this.dgvStudents);
            this.tabStudents.Controls.Add(this.panel1);
            this.tabStudents.Location = new System.Drawing.Point(4, 25);
            this.tabStudents.Name = "tabStudents";
            this.tabStudents.Size = new System.Drawing.Size(874, 524);
            this.tabStudents.TabIndex = 4;
            this.tabStudents.Text = "Учащиеся";
            this.tabStudents.UseVisualStyleBackColor = true;
            // 
            // dgvStudents
            // 
            this.dgvStudents.AllowUserToAddRows = false;
            this.dgvStudents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStudents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStudents.Location = new System.Drawing.Point(0, 130);
            this.dgvStudents.MultiSelect = false;
            this.dgvStudents.Name = "dgvStudents";
            this.dgvStudents.RowHeadersWidth = 51;
            this.dgvStudents.RowTemplate.Height = 24;
            this.dgvStudents.Size = new System.Drawing.Size(874, 394);
            this.dgvStudents.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblChangeGroup);
            this.panel1.Controls.Add(this.cmbChangeGroup);
            this.panel1.Controls.Add(this.btnViewProfile);
            this.panel1.Controls.Add(this.btnChangeGroup);
            this.panel1.Controls.Add(this.btnDeleteStudent);
            this.panel1.Controls.Add(this.btnAddStudent);
            this.panel1.Controls.Add(this.cmbParentUser);
            this.panel1.Controls.Add(this.lblParentUser);
            this.panel1.Controls.Add(this.cmbStudentUser);
            this.panel1.Controls.Add(this.lblStudentUser);
            this.panel1.Controls.Add(this.cmbStudentGroup);
            this.panel1.Controls.Add(this.lblGroup);
            this.panel1.Controls.Add(this.txtStudentFio);
            this.panel1.Controls.Add(this.lblFio);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(874, 130);
            this.panel1.TabIndex = 0;
            // 
            // lblChangeGroup
            // 
            this.lblChangeGroup.AutoSize = true;
            this.lblChangeGroup.Location = new System.Drawing.Point(520, 12);
            this.lblChangeGroup.Name = "lblChangeGroup";
            this.lblChangeGroup.Size = new System.Drawing.Size(100, 16);
            this.lblChangeGroup.TabIndex = 13;
            this.lblChangeGroup.Text = "Новая группа:";
            // 
            // cmbChangeGroup
            // 
            this.cmbChangeGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChangeGroup.FormattingEnabled = true;
            this.cmbChangeGroup.Location = new System.Drawing.Point(520, 37);
            this.cmbChangeGroup.Name = "cmbChangeGroup";
            this.cmbChangeGroup.Size = new System.Drawing.Size(150, 24);
            this.cmbChangeGroup.TabIndex = 12;
            // 
            // btnViewProfile
            // 
            this.btnViewProfile.Location = new System.Drawing.Point(300, 97);
            this.btnViewProfile.Name = "btnViewProfile";
            this.btnViewProfile.Size = new System.Drawing.Size(190, 23);
            this.btnViewProfile.TabIndex = 11;
            this.btnViewProfile.Text = "Просмотр профиля";
            this.btnViewProfile.UseVisualStyleBackColor = true;
            this.btnViewProfile.Click += new System.EventHandler(this.btnViewProfile_Click);
            // 
            // btnChangeGroup
            // 
            this.btnChangeGroup.Location = new System.Drawing.Point(300, 67);
            this.btnChangeGroup.Name = "btnChangeGroup";
            this.btnChangeGroup.Size = new System.Drawing.Size(190, 23);
            this.btnChangeGroup.TabIndex = 10;
            this.btnChangeGroup.Text = "Сменить группу";
            this.btnChangeGroup.UseVisualStyleBackColor = true;
            this.btnChangeGroup.Click += new System.EventHandler(this.btnChangeGroup_Click);
            // 
            // btnDeleteStudent
            // 
            this.btnDeleteStudent.Location = new System.Drawing.Point(300, 37);
            this.btnDeleteStudent.Name = "btnDeleteStudent";
            this.btnDeleteStudent.Size = new System.Drawing.Size(190, 23);
            this.btnDeleteStudent.TabIndex = 9;
            this.btnDeleteStudent.Text = "Удалить";
            this.btnDeleteStudent.UseVisualStyleBackColor = true;
            this.btnDeleteStudent.Click += new System.EventHandler(this.btnDeleteStudent_Click);
            // 
            // btnAddStudent
            // 
            this.btnAddStudent.Location = new System.Drawing.Point(300, 7);
            this.btnAddStudent.Name = "btnAddStudent";
            this.btnAddStudent.Size = new System.Drawing.Size(190, 23);
            this.btnAddStudent.TabIndex = 8;
            this.btnAddStudent.Text = "Добавить";
            this.btnAddStudent.UseVisualStyleBackColor = true;
            this.btnAddStudent.Click += new System.EventHandler(this.btnAddStudent_Click);
            // 
            // cmbParentUser
            // 
            this.cmbParentUser.FormattingEnabled = true;
            this.cmbParentUser.Location = new System.Drawing.Point(90, 97);
            this.cmbParentUser.Name = "cmbParentUser";
            this.cmbParentUser.Size = new System.Drawing.Size(180, 24);
            this.cmbParentUser.TabIndex = 7;
            // 
            // lblParentUser
            // 
            this.lblParentUser.AutoSize = true;
            this.lblParentUser.Location = new System.Drawing.Point(10, 100);
            this.lblParentUser.Name = "lblParentUser";
            this.lblParentUser.Size = new System.Drawing.Size(73, 16);
            this.lblParentUser.TabIndex = 6;
            this.lblParentUser.Text = "Родитель:";
            // 
            // cmbStudentUser
            // 
            this.cmbStudentUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStudentUser.FormattingEnabled = true;
            this.cmbStudentUser.Location = new System.Drawing.Point(110, 67);
            this.cmbStudentUser.Name = "cmbStudentUser";
            this.cmbStudentUser.Size = new System.Drawing.Size(160, 24);
            this.cmbStudentUser.TabIndex = 5;
            // 
            // lblStudentUser
            // 
            this.lblStudentUser.AutoSize = true;
            this.lblStudentUser.Location = new System.Drawing.Point(10, 70);
            this.lblStudentUser.Name = "lblStudentUser";
            this.lblStudentUser.Size = new System.Drawing.Size(95, 16);
            this.lblStudentUser.TabIndex = 4;
            this.lblStudentUser.Text = "Ученик (user):";
            // 
            // cmbStudentGroup
            // 
            this.cmbStudentGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStudentGroup.FormattingEnabled = true;
            this.cmbStudentGroup.Location = new System.Drawing.Point(70, 37);
            this.cmbStudentGroup.Name = "cmbStudentGroup";
            this.cmbStudentGroup.Size = new System.Drawing.Size(200, 24);
            this.cmbStudentGroup.TabIndex = 3;
            // 
            // lblGroup
            // 
            this.lblGroup.AutoSize = true;
            this.lblGroup.Location = new System.Drawing.Point(10, 40);
            this.lblGroup.Name = "lblGroup";
            this.lblGroup.Size = new System.Drawing.Size(57, 16);
            this.lblGroup.TabIndex = 2;
            this.lblGroup.Text = "Группа:";
            this.lblGroup.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtStudentFio
            // 
            this.txtStudentFio.Location = new System.Drawing.Point(70, 7);
            this.txtStudentFio.Name = "txtStudentFio";
            this.txtStudentFio.Size = new System.Drawing.Size(200, 22);
            this.txtStudentFio.TabIndex = 1;
            this.txtStudentFio.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // lblFio
            // 
            this.lblFio.AutoSize = true;
            this.lblFio.Location = new System.Drawing.Point(10, 10);
            this.lblFio.Name = "lblFio";
            this.lblFio.Size = new System.Drawing.Size(41, 16);
            this.lblFio.TabIndex = 0;
            this.lblFio.Text = "ФИО:";
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 553);
            this.Controls.Add(this.tabControl1);
            this.Name = "AdminForm";
            this.Text = "Панель администратора";
            this.tabControl1.ResumeLayout(false);
            this.tabUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.panelUsersTop.ResumeLayout(false);
            this.panelUsersTop.PerformLayout();
            this.tabGroups.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroups)).EndInit();
            this.panelGroupsTop.ResumeLayout(false);
            this.panelGroupsTop.PerformLayout();
            this.tabSubjects.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubjects)).EndInit();
            this.panelSubjectsTop.ResumeLayout(false);
            this.panelSubjectsTop.PerformLayout();
            this.tabSchedule.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSchedule)).EndInit();
            this.panelScheduleTop.ResumeLayout(false);
            this.tabStudents.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabUsers;
        private System.Windows.Forms.TabPage tabGroups;
        private System.Windows.Forms.TabPage tabSubjects;
        private System.Windows.Forms.TabPage tabSchedule;
        private System.Windows.Forms.Panel panelUsersTop;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.ComboBox cmbRole;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.Button btnUpdateRole;
        private System.Windows.Forms.Button btnDeleteUser;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.Panel panelGroupsTop;
        private System.Windows.Forms.TextBox txtGroupName;
        private System.Windows.Forms.Button btnDeleteGroup;
        private System.Windows.Forms.Button btnAddGroup;
        private System.Windows.Forms.DataGridView dgvGroups;
        private System.Windows.Forms.Panel panelSubjectsTop;
        private System.Windows.Forms.DataGridView dgvSubjects;
        private System.Windows.Forms.Button btnDeleteSubject;
        private System.Windows.Forms.Button btnAddSubject;
        private System.Windows.Forms.TextBox txtSubjectPrice;
        private System.Windows.Forms.TextBox txtSubjectName;
        private System.Windows.Forms.Panel panelScheduleTop;
        private System.Windows.Forms.ComboBox cmbLesson;
        private System.Windows.Forms.Button btnDeleteSchedule;
        private System.Windows.Forms.Button btnUpdateSchedule;
        private System.Windows.Forms.ComboBox cmbDay;
        private System.Windows.Forms.DataGridView dgvSchedule;
        private System.Windows.Forms.TabPage tabStudents;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblFio;
        private System.Windows.Forms.TextBox txtStudentFio;
        private System.Windows.Forms.Label lblGroup;
        private System.Windows.Forms.ComboBox cmbStudentGroup;
        private System.Windows.Forms.ComboBox cmbStudentUser;
        private System.Windows.Forms.Label lblStudentUser;
        private System.Windows.Forms.Label lblParentUser;
        private System.Windows.Forms.ComboBox cmbParentUser;
        private System.Windows.Forms.Button btnViewProfile;
        private System.Windows.Forms.Button btnChangeGroup;
        private System.Windows.Forms.Button btnDeleteStudent;
        private System.Windows.Forms.Button btnAddStudent;
        private System.Windows.Forms.ComboBox cmbChangeGroup;
        private System.Windows.Forms.Label lblChangeGroup;
        private System.Windows.Forms.DataGridView dgvStudents;
        private System.Windows.Forms.Button btnUpdateGroup;
        private System.Windows.Forms.TextBox txtEditGroupName;
        private System.Windows.Forms.Button btnUpdateSubject;
        private System.Windows.Forms.TextBox txtEditSubjectPrice;
        private System.Windows.Forms.TextBox txtEditSubjectName;
    }
}