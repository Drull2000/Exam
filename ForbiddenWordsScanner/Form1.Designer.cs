namespace ForbiddenWordsScanner
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtWords = new TextBox();
            knopkaStart = new Button();
            knopkaPaysa = new Button();
            knopkaOstan = new Button();
            progressBar1 = new ProgressBar();
            listBoxLog = new ListBox();
            lblStatus = new Label();
            txtPath = new TextBox();
            btnBrowse = new Button();
            SuspendLayout();
            // 
            // txtWords
            // 
            txtWords.Location = new Point(159, 117);
            txtWords.Multiline = true;
            txtWords.Name = "txtWords";
            txtWords.Size = new Size(448, 23);
            txtWords.TabIndex = 0;
            // 
            // knopkaStart
            // 
            knopkaStart.Location = new Point(136, 162);
            knopkaStart.Name = "knopkaStart";
            knopkaStart.Size = new Size(75, 23);
            knopkaStart.TabIndex = 1;
            knopkaStart.Text = "Стартуй";
            knopkaStart.UseVisualStyleBackColor = true;
            knopkaStart.Click += knopkaStart_Click;
            // 
            // knopkaPaysa
            // 
            knopkaPaysa.Location = new Point(350, 162);
            knopkaPaysa.Name = "knopkaPaysa";
            knopkaPaysa.Size = new Size(75, 23);
            knopkaPaysa.TabIndex = 2;
            knopkaPaysa.Text = "Паузуй";
            knopkaPaysa.UseVisualStyleBackColor = true;
            knopkaPaysa.Click += knopkaPaysa_Click;
            // 
            // knopkaOstan
            // 
            knopkaOstan.Location = new Point(541, 162);
            knopkaOstan.Name = "knopkaOstan";
            knopkaOstan.Size = new Size(75, 23);
            knopkaOstan.TabIndex = 3;
            knopkaOstan.Text = "Астанавись";
            knopkaOstan.UseVisualStyleBackColor = true;
            knopkaOstan.Click += knopkaOstan_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(83, 276);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(619, 23);
            progressBar1.TabIndex = 4;
            // 
            // listBoxLog
            // 
            listBoxLog.FormattingEnabled = true;
            listBoxLog.Location = new Point(83, 327);
            listBoxLog.Name = "listBoxLog";
            listBoxLog.Size = new Size(619, 94);
            listBoxLog.TabIndex = 5;
            listBoxLog.SelectedIndexChanged += listBoxFiles_SelectedIndexChanged;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(390, 245);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(16, 15);
            lblStatus.TabIndex = 6;
            lblStatus.Text = "...";
            // 
            // txtPath
            // 
            txtPath.Location = new Point(190, 67);
            txtPath.Multiline = true;
            txtPath.Name = "txtPath";
            txtPath.Size = new Size(390, 23);
            txtPath.TabIndex = 7;
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(136, 204);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(471, 23);
            btnBrowse.TabIndex = 8;
            btnBrowse.Text = "Выбрать Путь";
            btnBrowse.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnBrowse);
            Controls.Add(txtPath);
            Controls.Add(lblStatus);
            Controls.Add(listBoxLog);
            Controls.Add(progressBar1);
            Controls.Add(knopkaOstan);
            Controls.Add(knopkaPaysa);
            Controls.Add(knopkaStart);
            Controls.Add(txtWords);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtWords;
        private Button knopkaStart;
        private Button knopkaPaysa;
        private Button knopkaOstan;
        private ProgressBar progressBar1;
        private ListBox listBoxLog;
        private Label lblStatus;
        private TextBox txtPath;
        private Button btnBrowse;
    }
}
