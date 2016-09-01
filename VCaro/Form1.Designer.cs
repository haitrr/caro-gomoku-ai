namespace VCaro
{
    partial class FVCaro
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
            this.components = new System.ComponentModel.Container();
            this.BTNew = new System.Windows.Forms.Button();
            this.BTUndo = new System.Windows.Forms.Button();
            this.BTExit = new System.Windows.Forms.Button();
            this.PBCPU = new System.Windows.Forms.PictureBox();
            this.PBHM = new System.Windows.Forms.PictureBox();
            this.PNCaroBoard = new System.Windows.Forms.Panel();
            this.BTHelp = new System.Windows.Forms.Button();
            this.BTHM = new System.Windows.Forms.Button();
            this.LBCPUTime = new System.Windows.Forms.Label();
            this.TMMoveTime = new System.Windows.Forms.Timer(this.components);
            this.LBHMTime = new System.Windows.Forms.Label();
            this.GBDifficulty = new System.Windows.Forms.GroupBox();
            this.RBVeryHard = new System.Windows.Forms.RadioButton();
            this.RBHard = new System.Windows.Forms.RadioButton();
            this.RBNormal = new System.Windows.Forms.RadioButton();
            this.RBEasy = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.PBCPU)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PBHM)).BeginInit();
            this.GBDifficulty.SuspendLayout();
            this.SuspendLayout();
            // 
            // BTNew
            // 
            this.BTNew.Location = new System.Drawing.Point(12, 594);
            this.BTNew.Name = "BTNew";
            this.BTNew.Size = new System.Drawing.Size(75, 23);
            this.BTNew.TabIndex = 1;
            this.BTNew.Text = "New Game";
            this.BTNew.UseVisualStyleBackColor = true;
            this.BTNew.Click += new System.EventHandler(this.BTNew_Click);
            // 
            // BTUndo
            // 
            this.BTUndo.Location = new System.Drawing.Point(260, 594);
            this.BTUndo.Name = "BTUndo";
            this.BTUndo.Size = new System.Drawing.Size(75, 23);
            this.BTUndo.TabIndex = 2;
            this.BTUndo.Text = "Undo";
            this.BTUndo.UseVisualStyleBackColor = true;
            this.BTUndo.Click += new System.EventHandler(this.BTUndo_Click);
            // 
            // BTExit
            // 
            this.BTExit.Location = new System.Drawing.Point(688, 594);
            this.BTExit.Name = "BTExit";
            this.BTExit.Size = new System.Drawing.Size(75, 23);
            this.BTExit.TabIndex = 4;
            this.BTExit.Text = "Exit";
            this.BTExit.UseVisualStyleBackColor = true;
            this.BTExit.Click += new System.EventHandler(this.BTExit_Click);
            // 
            // PBCPU
            // 
            this.PBCPU.Location = new System.Drawing.Point(635, 36);
            this.PBCPU.Name = "PBCPU";
            this.PBCPU.Size = new System.Drawing.Size(135, 135);
            this.PBCPU.TabIndex = 5;
            this.PBCPU.TabStop = false;
            // 
            // PBHM
            // 
            this.PBHM.Location = new System.Drawing.Point(635, 361);
            this.PBHM.Name = "PBHM";
            this.PBHM.Size = new System.Drawing.Size(135, 135);
            this.PBHM.TabIndex = 6;
            this.PBHM.TabStop = false;
            // 
            // PNCaroBoard
            // 
            this.PNCaroBoard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.PNCaroBoard.Location = new System.Drawing.Point(12, 12);
            this.PNCaroBoard.Name = "PNCaroBoard";
            this.PNCaroBoard.Size = new System.Drawing.Size(576, 576);
            this.PNCaroBoard.TabIndex = 7;
            this.PNCaroBoard.Paint += new System.Windows.Forms.PaintEventHandler(this.PNCaroBoard_Paint);
            this.PNCaroBoard.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PNCaroBoard_MouseClick);
            // 
            // BTHelp
            // 
            this.BTHelp.Location = new System.Drawing.Point(473, 594);
            this.BTHelp.Name = "BTHelp";
            this.BTHelp.Size = new System.Drawing.Size(75, 23);
            this.BTHelp.TabIndex = 8;
            this.BTHelp.Text = "Help";
            this.BTHelp.UseVisualStyleBackColor = true;
            this.BTHelp.Click += new System.EventHandler(this.BTHelp_Click);
            // 
            // BTHM
            // 
            this.BTHM.Location = new System.Drawing.Point(635, 7);
            this.BTHM.Name = "BTHM";
            this.BTHM.Size = new System.Drawing.Size(135, 23);
            this.BTHM.TabIndex = 10;
            this.BTHM.Text = "HelpMe";
            this.BTHM.UseVisualStyleBackColor = true;
            this.BTHM.Click += new System.EventHandler(this.BTHM_Click);
            // 
            // LBCPUTime
            // 
            this.LBCPUTime.AutoSize = true;
            this.LBCPUTime.BackColor = System.Drawing.Color.Transparent;
            this.LBCPUTime.Location = new System.Drawing.Point(685, 174);
            this.LBCPUTime.Name = "LBCPUTime";
            this.LBCPUTime.Size = new System.Drawing.Size(22, 13);
            this.LBCPUTime.TabIndex = 11;
            this.LBCPUTime.Text = "--:--";
            // 
            // TMMoveTime
            // 
            this.TMMoveTime.Enabled = true;
            this.TMMoveTime.Interval = 1000;
            this.TMMoveTime.Tick += new System.EventHandler(this.TMMoveTime_Tick);
            // 
            // LBHMTime
            // 
            this.LBHMTime.AutoSize = true;
            this.LBHMTime.BackColor = System.Drawing.Color.Transparent;
            this.LBHMTime.Location = new System.Drawing.Point(685, 528);
            this.LBHMTime.Name = "LBHMTime";
            this.LBHMTime.Size = new System.Drawing.Size(22, 13);
            this.LBHMTime.TabIndex = 12;
            this.LBHMTime.Text = "--:--";
            // 
            // GBDifficulty
            // 
            this.GBDifficulty.BackColor = System.Drawing.Color.Transparent;
            this.GBDifficulty.Controls.Add(this.RBVeryHard);
            this.GBDifficulty.Controls.Add(this.RBHard);
            this.GBDifficulty.Controls.Add(this.RBNormal);
            this.GBDifficulty.Controls.Add(this.RBEasy);
            this.GBDifficulty.Location = new System.Drawing.Point(628, 203);
            this.GBDifficulty.Name = "GBDifficulty";
            this.GBDifficulty.Size = new System.Drawing.Size(149, 107);
            this.GBDifficulty.TabIndex = 14;
            this.GBDifficulty.TabStop = false;
            this.GBDifficulty.Text = "Độ khó";
            // 
            // RBVeryHard
            // 
            this.RBVeryHard.AutoSize = true;
            this.RBVeryHard.Location = new System.Drawing.Point(7, 83);
            this.RBVeryHard.Name = "RBVeryHard";
            this.RBVeryHard.Size = new System.Drawing.Size(63, 17);
            this.RBVeryHard.TabIndex = 3;
            this.RBVeryHard.TabStop = true;
            this.RBVeryHard.Text = "Rất khó";
            this.RBVeryHard.UseVisualStyleBackColor = true;
            // 
            // RBHard
            // 
            this.RBHard.AutoSize = true;
            this.RBHard.Location = new System.Drawing.Point(7, 60);
            this.RBHard.Name = "RBHard";
            this.RBHard.Size = new System.Drawing.Size(44, 17);
            this.RBHard.TabIndex = 2;
            this.RBHard.TabStop = true;
            this.RBHard.Text = "Khó";
            this.RBHard.UseVisualStyleBackColor = true;
            // 
            // RBNormal
            // 
            this.RBNormal.AutoSize = true;
            this.RBNormal.Location = new System.Drawing.Point(7, 42);
            this.RBNormal.Name = "RBNormal";
            this.RBNormal.Size = new System.Drawing.Size(62, 17);
            this.RBNormal.TabIndex = 1;
            this.RBNormal.TabStop = true;
            this.RBNormal.Text = "Thường";
            this.RBNormal.UseVisualStyleBackColor = true;
            // 
            // RBEasy
            // 
            this.RBEasy.AutoSize = true;
            this.RBEasy.Location = new System.Drawing.Point(7, 20);
            this.RBEasy.Name = "RBEasy";
            this.RBEasy.Size = new System.Drawing.Size(39, 17);
            this.RBEasy.TabIndex = 0;
            this.RBEasy.TabStop = true;
            this.RBEasy.Text = "Dễ";
            this.RBEasy.UseVisualStyleBackColor = true;
            // 
            // FVCaro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(801, 638);
            this.Controls.Add(this.GBDifficulty);
            this.Controls.Add(this.LBHMTime);
            this.Controls.Add(this.LBCPUTime);
            this.Controls.Add(this.BTHM);
            this.Controls.Add(this.BTHelp);
            this.Controls.Add(this.PNCaroBoard);
            this.Controls.Add(this.PBHM);
            this.Controls.Add(this.PBCPU);
            this.Controls.Add(this.BTExit);
            this.Controls.Add(this.BTUndo);
            this.Controls.Add(this.BTNew);
            this.Name = "FVCaro";
            this.Text = "Caro";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PBCPU)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PBHM)).EndInit();
            this.GBDifficulty.ResumeLayout(false);
            this.GBDifficulty.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BTNew;
        private System.Windows.Forms.Button BTUndo;
        private System.Windows.Forms.Button BTExit;
        private System.Windows.Forms.PictureBox PBCPU;
        private System.Windows.Forms.PictureBox PBHM;
        private System.Windows.Forms.Panel PNCaroBoard;
        private System.Windows.Forms.Button BTHelp;
        private System.Windows.Forms.Button BTHM;
        private System.Windows.Forms.Label LBCPUTime;
        private System.Windows.Forms.Timer TMMoveTime;
        private System.Windows.Forms.Label LBHMTime;
        private System.Windows.Forms.GroupBox GBDifficulty;
        private System.Windows.Forms.RadioButton RBVeryHard;
        private System.Windows.Forms.RadioButton RBHard;
        private System.Windows.Forms.RadioButton RBNormal;
        private System.Windows.Forms.RadioButton RBEasy;
    }
}

