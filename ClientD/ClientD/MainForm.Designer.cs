namespace ClientD
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBoxSubjects = new System.Windows.Forms.ListBox();
            this.listBoxLogbooks = new System.Windows.Forms.ListBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.buttonCalculateCost = new System.Windows.Forms.Button();
            this.BtnAddReview = new System.Windows.Forms.Button();
            this.BtnRating = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxSubjects
            // 
            this.listBoxSubjects.FormattingEnabled = true;
            this.listBoxSubjects.ItemHeight = 16;
            this.listBoxSubjects.Location = new System.Drawing.Point(42, 45);
            this.listBoxSubjects.Name = "listBoxSubjects";
            this.listBoxSubjects.Size = new System.Drawing.Size(120, 84);
            this.listBoxSubjects.TabIndex = 0;
            this.listBoxSubjects.SelectedIndexChanged += new System.EventHandler(this.listBoxSubjects_SelectedIndexChanged);
            // 
            // listBoxLogbooks
            // 
            this.listBoxLogbooks.FormattingEnabled = true;
            this.listBoxLogbooks.ItemHeight = 16;
            this.listBoxLogbooks.Location = new System.Drawing.Point(42, 177);
            this.listBoxLogbooks.Name = "listBoxLogbooks";
            this.listBoxLogbooks.Size = new System.Drawing.Size(120, 84);
            this.listBoxLogbooks.TabIndex = 1;
            this.listBoxLogbooks.SelectedIndexChanged += new System.EventHandler(this.listBoxLogbooks_SelectedIndexChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(199, 45);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1287, 622);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewJournal_CellValueChanged);
            // 
            // buttonCalculateCost
            // 
            this.buttonCalculateCost.Location = new System.Drawing.Point(12, 310);
            this.buttonCalculateCost.Name = "buttonCalculateCost";
            this.buttonCalculateCost.Size = new System.Drawing.Size(170, 36);
            this.buttonCalculateCost.TabIndex = 3;
            this.buttonCalculateCost.Text = "Просмотр стоимости";
            this.buttonCalculateCost.UseVisualStyleBackColor = true;
            this.buttonCalculateCost.Click += new System.EventHandler(this.buttonCalculateCost_Click);
            // 
            // BtnAddReview
            // 
            this.BtnAddReview.Location = new System.Drawing.Point(13, 352);
            this.BtnAddReview.Name = "BtnAddReview";
            this.BtnAddReview.Size = new System.Drawing.Size(169, 36);
            this.BtnAddReview.TabIndex = 4;
            this.BtnAddReview.Text = "Оставить отзыв";
            this.BtnAddReview.UseVisualStyleBackColor = true;
            this.BtnAddReview.Click += new System.EventHandler(this.BtnAddReview_Click);
            // 
            // BtnRating
            // 
            this.BtnRating.Location = new System.Drawing.Point(12, 394);
            this.BtnRating.Name = "BtnRating";
            this.BtnRating.Size = new System.Drawing.Size(169, 36);
            this.BtnRating.TabIndex = 5;
            this.BtnRating.Text = "Рейтинг";
            this.BtnRating.UseVisualStyleBackColor = true;
            this.BtnRating.Click += new System.EventHandler(this.BtnRating_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1526, 699);
            this.Controls.Add(this.BtnRating);
            this.Controls.Add(this.BtnAddReview);
            this.Controls.Add(this.buttonCalculateCost);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.listBoxLogbooks);
            this.Controls.Add(this.listBoxSubjects);
            this.Name = "MainForm";
            this.Text = "Журнал";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxSubjects;
        private System.Windows.Forms.ListBox listBoxLogbooks;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonCalculateCost;
        private System.Windows.Forms.Button BtnAddReview;
        private System.Windows.Forms.Button BtnRating;
    }
}

