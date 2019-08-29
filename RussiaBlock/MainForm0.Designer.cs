namespace RussiaBlock
{
    partial class MainForm0
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm0));
            this.label1 = new System.Windows.Forms.Label();
            this.single = new System.Windows.Forms.Button();
            this.couple = new System.Windows.Forms.Button();
            this.music = new System.Windows.Forms.Button();
            this.help3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.music0 = new AxWMPLib.AxWindowsMediaPlayer();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.eventLog1 = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.music0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("华文楷体", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.DarkMagenta;
            this.label1.Location = new System.Drawing.Point(127, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 39);
            this.label1.TabIndex = 0;
            this.label1.Text = "牛逼119";
            // 
            // single
            // 
            this.single.FlatAppearance.BorderSize = 0;
            this.single.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.single.Font = new System.Drawing.Font("Segoe UI Symbol", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.single.ForeColor = System.Drawing.Color.MediumVioletRed;
            this.single.Image = global::RussiaBlock.Properties.Resources._33;
            this.single.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.single.Location = new System.Drawing.Point(125, 234);
            this.single.Name = "single";
            this.single.Size = new System.Drawing.Size(141, 51);
            this.single.TabIndex = 1;
            this.single.Text = "Single";
            this.single.UseVisualStyleBackColor = true;
            this.single.Click += new System.EventHandler(this.single_Click);
            // 
            // couple
            // 
            this.couple.FlatAppearance.BorderSize = 0;
            this.couple.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.couple.Font = new System.Drawing.Font("Segoe UI Symbol", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.couple.ForeColor = System.Drawing.Color.MediumVioletRed;
            this.couple.Image = global::RussiaBlock.Properties.Resources._33;
            this.couple.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.couple.Location = new System.Drawing.Point(125, 324);
            this.couple.Name = "couple";
            this.couple.Size = new System.Drawing.Size(142, 51);
            this.couple.TabIndex = 2;
            this.couple.Text = "Two-player";
            this.couple.UseVisualStyleBackColor = true;
            this.couple.Click += new System.EventHandler(this.couple_Click);
            // 
            // music
            // 
            this.music.FlatAppearance.BorderSize = 0;
            this.music.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.music.ForeColor = System.Drawing.SystemColors.Control;
            this.music.Image = global::RussiaBlock.Properties.Resources.Apple_Music;
            this.music.Location = new System.Drawing.Point(309, 12);
            this.music.Name = "music";
            this.music.Size = new System.Drawing.Size(33, 31);
            this.music.TabIndex = 3;
            this.music.Text = "1";
            this.music.UseVisualStyleBackColor = true;
            this.music.Click += new System.EventHandler(this.music_Click);
            // 
            // help3
            // 
            this.help3.FlatAppearance.BorderSize = 0;
            this.help3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.help3.Image = global::RussiaBlock.Properties.Resources._12;
            this.help3.Location = new System.Drawing.Point(359, 12);
            this.help3.Name = "help3";
            this.help3.Size = new System.Drawing.Size(32, 31);
            this.help3.TabIndex = 4;
            this.help3.UseVisualStyleBackColor = true;
            this.help3.Click += new System.EventHandler(this.help3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("华文楷体", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.DarkMagenta;
            this.label2.Location = new System.Drawing.Point(97, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(197, 39);
            this.label2.TabIndex = 5;
            this.label2.Text = "俄罗斯方块";
            // 
            // music0
            // 
            this.music0.Enabled = true;
            this.music0.Location = new System.Drawing.Point(309, 190);
            this.music0.Name = "music0";
            this.music0.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("music0.OcxState")));
            this.music0.Size = new System.Drawing.Size(0, 0);
            this.music0.TabIndex = 6;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // eventLog1
            // 
            this.eventLog1.SynchronizingObject = this;
            this.eventLog1.EntryWritten += new System.Diagnostics.EntryWrittenEventHandler(this.eventLog1_EntryWritten);
            // 
            // MainForm0
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::RussiaBlock.Properties.Resources._33;
            this.ClientSize = new System.Drawing.Size(403, 468);
            this.Controls.Add(this.music0);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.help3);
            this.Controls.Add(this.music);
            this.Controls.Add(this.couple);
            this.Controls.Add(this.single);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm0";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "俄罗斯方块";
            this.Load += new System.EventHandler(this.MainForm0_Load);
            ((System.ComponentModel.ISupportInitialize)(this.music0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button single;
        private System.Windows.Forms.Button couple;
        private System.Windows.Forms.Button music;
        private System.Windows.Forms.Button help3;
        private System.Windows.Forms.Label label2;
        private AxWMPLib.AxWindowsMediaPlayer music0;
        private System.Windows.Forms.Timer timer1;
        private System.Diagnostics.EventLog eventLog1;
    }
}

