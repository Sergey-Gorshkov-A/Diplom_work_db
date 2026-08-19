namespace ClientD
{
    partial class ParentForm
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
            this.lstChildren = new System.Windows.Forms.ListBox();
            this.dgvChildMarks = new System.Windows.Forms.DataGridView();
            this.dgvAttendance = new System.Windows.Forms.DataGridView();
            this.lstReviews = new System.Windows.Forms.ListBox();
            this.btnEditProfile = new System.Windows.Forms.Button();
            this.btnPayLessons = new System.Windows.Forms.Button();
            this.btnAddBalance = new System.Windows.Forms.Button();
            this.lblBalance = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChildMarks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttendance)).BeginInit();
            this.SuspendLayout();
            // 
            // lstChildren
            // 
            this.lstChildren.FormattingEnabled = true;
            this.lstChildren.ItemHeight = 16;
            this.lstChildren.Location = new System.Drawing.Point(36, 37);
            this.lstChildren.Name = "lstChildren";
            this.lstChildren.Size = new System.Drawing.Size(250, 84);
            this.lstChildren.TabIndex = 0;
            this.lstChildren.SelectedIndexChanged += new System.EventHandler(this.lstChildren_SelectedIndexChanged);
            // 
            // dgvChildMarks
            // 
            this.dgvChildMarks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChildMarks.Location = new System.Drawing.Point(326, 37);
            this.dgvChildMarks.Name = "dgvChildMarks";
            this.dgvChildMarks.RowHeadersWidth = 51;
            this.dgvChildMarks.RowTemplate.Height = 24;
            this.dgvChildMarks.Size = new System.Drawing.Size(296, 260);
            this.dgvChildMarks.TabIndex = 1;
            // 
            // dgvAttendance
            // 
            this.dgvAttendance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAttendance.Location = new System.Drawing.Point(638, 37);
            this.dgvAttendance.Name = "dgvAttendance";
            this.dgvAttendance.RowHeadersWidth = 51;
            this.dgvAttendance.RowTemplate.Height = 24;
            this.dgvAttendance.Size = new System.Drawing.Size(296, 260);
            this.dgvAttendance.TabIndex = 2;
            // 
            // lstReviews
            // 
            this.lstReviews.FormattingEnabled = true;
            this.lstReviews.ItemHeight = 16;
            this.lstReviews.Location = new System.Drawing.Point(36, 322);
            this.lstReviews.Name = "lstReviews";
            this.lstReviews.Size = new System.Drawing.Size(898, 116);
            this.lstReviews.TabIndex = 3;
            // 
            // btnEditProfile
            // 
            this.btnEditProfile.Location = new System.Drawing.Point(36, 258);
            this.btnEditProfile.Name = "btnEditProfile";
            this.btnEditProfile.Size = new System.Drawing.Size(250, 38);
            this.btnEditProfile.TabIndex = 4;
            this.btnEditProfile.Text = "Изменить профиль";
            this.btnEditProfile.UseVisualStyleBackColor = true;
            this.btnEditProfile.Click += new System.EventHandler(this.btnEditProfile_Click);
            // 
            // btnPayLessons
            // 
            this.btnPayLessons.Location = new System.Drawing.Point(36, 214);
            this.btnPayLessons.Name = "btnPayLessons";
            this.btnPayLessons.Size = new System.Drawing.Size(250, 38);
            this.btnPayLessons.TabIndex = 5;
            this.btnPayLessons.Text = "Оплатить уроки";
            this.btnPayLessons.UseVisualStyleBackColor = true;
            this.btnPayLessons.Click += new System.EventHandler(this.btnPayLessons_Click);
            // 
            // btnAddBalance
            // 
            this.btnAddBalance.Location = new System.Drawing.Point(36, 127);
            this.btnAddBalance.Name = "btnAddBalance";
            this.btnAddBalance.Size = new System.Drawing.Size(250, 38);
            this.btnAddBalance.TabIndex = 6;
            this.btnAddBalance.Text = "Пополнить счёт";
            this.btnAddBalance.UseVisualStyleBackColor = true;
            this.btnAddBalance.Click += new System.EventHandler(this.btnAddBalance_Click);
            // 
            // lblBalance
            // 
            this.lblBalance.AutoSize = true;
            this.lblBalance.Location = new System.Drawing.Point(137, 181);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(44, 16);
            this.lblBalance.TabIndex = 7;
            this.lblBalance.Text = "label1";
            // 
            // ParentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(957, 450);
            this.Controls.Add(this.lblBalance);
            this.Controls.Add(this.btnAddBalance);
            this.Controls.Add(this.btnPayLessons);
            this.Controls.Add(this.btnEditProfile);
            this.Controls.Add(this.lstReviews);
            this.Controls.Add(this.dgvAttendance);
            this.Controls.Add(this.dgvChildMarks);
            this.Controls.Add(this.lstChildren);
            this.Name = "ParentForm";
            this.Text = "Родительский кабинет";
            ((System.ComponentModel.ISupportInitialize)(this.dgvChildMarks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttendance)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstChildren;
        private System.Windows.Forms.DataGridView dgvChildMarks;
        private System.Windows.Forms.DataGridView dgvAttendance;
        private System.Windows.Forms.ListBox lstReviews;
        private System.Windows.Forms.Button btnEditProfile;
        private System.Windows.Forms.Button btnPayLessons;
        private System.Windows.Forms.Button btnAddBalance;
        private System.Windows.Forms.Label lblBalance;
    }
}