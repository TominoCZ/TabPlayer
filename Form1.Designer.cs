
namespace TabPlayer
{
	partial class Form1
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.rtbTab = new System.Windows.Forms.RichTextBox();
			this.btnPlayPause = new System.Windows.Forms.Button();
			this.tbarSpeed = new System.Windows.Forms.TrackBar();
			this.lblTab = new System.Windows.Forms.Label();
			this.pTab = new System.Windows.Forms.Panel();
			this.pCenter = new System.Windows.Forms.Panel();
			this.chbRepeat = new System.Windows.Forms.CheckBox();
			this.updateTimer = new System.Windows.Forms.Timer(this.components);
			this.lblSpeed = new System.Windows.Forms.Label();
			this.btnStop = new System.Windows.Forms.Button();
			this.pContainer = new System.Windows.Forms.Panel();
			this.pProgress = new System.Windows.Forms.Panel();
			this.chbPauseOnEdit = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.tbarSpeed)).BeginInit();
			this.pTab.SuspendLayout();
			this.pContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// rtbTab
			// 
			this.rtbTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rtbTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
			this.rtbTab.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rtbTab.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.rtbTab.ForeColor = System.Drawing.SystemColors.Menu;
			this.rtbTab.Location = new System.Drawing.Point(0, 0);
			this.rtbTab.Name = "rtbTab";
			this.rtbTab.Size = new System.Drawing.Size(478, 185);
			this.rtbTab.TabIndex = 0;
			this.rtbTab.Text = resources.GetString("rtbTab.Text");
			this.rtbTab.WordWrap = false;
			this.rtbTab.TextChanged += new System.EventHandler(this.rtbTab_TextChanged);
			// 
			// btnPlayPause
			// 
			this.btnPlayPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnPlayPause.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.btnPlayPause.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.btnPlayPause.FlatAppearance.BorderColor = System.Drawing.Color.White;
			this.btnPlayPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnPlayPause.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.btnPlayPause.ForeColor = System.Drawing.SystemColors.Control;
			this.btnPlayPause.Location = new System.Drawing.Point(5, 330);
			this.btnPlayPause.Name = "btnPlayPause";
			this.btnPlayPause.Size = new System.Drawing.Size(130, 38);
			this.btnPlayPause.TabIndex = 1;
			this.btnPlayPause.Text = "PLAY";
			this.btnPlayPause.UseMnemonic = false;
			this.btnPlayPause.UseVisualStyleBackColor = false;
			this.btnPlayPause.Click += new System.EventHandler(this.btnPlayPause_Click);
			// 
			// tbarSpeed
			// 
			this.tbarSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbarSpeed.AutoSize = false;
			this.tbarSpeed.LargeChange = 0;
			this.tbarSpeed.Location = new System.Drawing.Point(185, 341);
			this.tbarSpeed.Maximum = 200;
			this.tbarSpeed.Minimum = 25;
			this.tbarSpeed.Name = "tbarSpeed";
			this.tbarSpeed.Size = new System.Drawing.Size(293, 33);
			this.tbarSpeed.SmallChange = 0;
			this.tbarSpeed.TabIndex = 2;
			this.tbarSpeed.TickFrequency = 0;
			this.tbarSpeed.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.tbarSpeed.Value = 100;
			this.tbarSpeed.Scroll += new System.EventHandler(this.tbarSpeed_ValueChanged);
			this.tbarSpeed.ValueChanged += new System.EventHandler(this.tbarSpeed_ValueChanged);
			this.tbarSpeed.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbarSpeed_MouseUp);
			// 
			// lblTab
			// 
			this.lblTab.BackColor = System.Drawing.Color.Transparent;
			this.lblTab.Font = new System.Drawing.Font("Consolas", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lblTab.ForeColor = System.Drawing.SystemColors.Menu;
			this.lblTab.Location = new System.Drawing.Point(0, 0);
			this.lblTab.Name = "lblTab";
			this.lblTab.Size = new System.Drawing.Size(732, 24);
			this.lblTab.TabIndex = 5;
			this.lblTab.Click += new System.EventHandler(this.LoseTextBoxFocus);
			this.lblTab.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LoseTextBoxFocus);
			this.lblTab.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblTab_MouseDown);
			this.lblTab.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblTab_MouseUp);
			// 
			// pTab
			// 
			this.pTab.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
			this.pTab.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pTab.Controls.Add(this.pCenter);
			this.pTab.Controls.Add(this.lblTab);
			this.pTab.Location = new System.Drawing.Point(0, 185);
			this.pTab.Name = "pTab";
			this.pTab.Size = new System.Drawing.Size(478, 133);
			this.pTab.TabIndex = 6;
			this.pTab.Click += new System.EventHandler(this.LoseTextBoxFocus);
			this.pTab.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LoseTextBoxFocus);
			this.pTab.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblTab_MouseDown);
			this.pTab.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblTab_MouseUp);
			// 
			// pCenter
			// 
			this.pCenter.BackColor = System.Drawing.Color.Yellow;
			this.pCenter.Location = new System.Drawing.Point(309, 0);
			this.pCenter.Name = "pCenter";
			this.pCenter.Size = new System.Drawing.Size(2, 133);
			this.pCenter.TabIndex = 6;
			// 
			// chbRepeat
			// 
			this.chbRepeat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chbRepeat.AutoSize = true;
			this.chbRepeat.Checked = true;
			this.chbRepeat.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbRepeat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.chbRepeat.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.chbRepeat.ForeColor = System.Drawing.SystemColors.Control;
			this.chbRepeat.Location = new System.Drawing.Point(12, 379);
			this.chbRepeat.Name = "chbRepeat";
			this.chbRepeat.Size = new System.Drawing.Size(65, 19);
			this.chbRepeat.TabIndex = 7;
			this.chbRepeat.Text = "Repeat";
			this.chbRepeat.UseVisualStyleBackColor = true;
			this.chbRepeat.CheckedChanged += new System.EventHandler(this.chbRepeat_CheckedChanged);
			// 
			// updateTimer
			// 
			this.updateTimer.Enabled = true;
			this.updateTimer.Interval = 15;
			this.updateTimer.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// lblSpeed
			// 
			this.lblSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblSpeed.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.lblSpeed.ForeColor = System.Drawing.SystemColors.Control;
			this.lblSpeed.Location = new System.Drawing.Point(191, 330);
			this.lblSpeed.Name = "lblSpeed";
			this.lblSpeed.Size = new System.Drawing.Size(287, 22);
			this.lblSpeed.TabIndex = 8;
			this.lblSpeed.Text = "Speed: 100%";
			// 
			// btnStop
			// 
			this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnStop.BackColor = System.Drawing.Color.Red;
			this.btnStop.FlatAppearance.BorderColor = System.Drawing.Color.White;
			this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnStop.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.btnStop.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnStop.Location = new System.Drawing.Point(141, 330);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(38, 38);
			this.btnStop.TabIndex = 9;
			this.btnStop.UseMnemonic = false;
			this.btnStop.UseVisualStyleBackColor = false;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// pContainer
			// 
			this.pContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.pContainer.Controls.Add(this.pProgress);
			this.pContainer.Controls.Add(this.rtbTab);
			this.pContainer.Controls.Add(this.pTab);
			this.pContainer.Location = new System.Drawing.Point(0, 0);
			this.pContainer.Name = "pContainer";
			this.pContainer.Size = new System.Drawing.Size(478, 324);
			this.pContainer.TabIndex = 7;
			this.pContainer.Click += new System.EventHandler(this.LoseTextBoxFocus);
			this.pContainer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LoseTextBoxFocus);
			// 
			// pProgress
			// 
			this.pProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pProgress.BackColor = System.Drawing.Color.Red;
			this.pProgress.Location = new System.Drawing.Point(0, 318);
			this.pProgress.Name = "pProgress";
			this.pProgress.Size = new System.Drawing.Size(228, 6);
			this.pProgress.TabIndex = 7;
			this.pProgress.Click += new System.EventHandler(this.LoseTextBoxFocus);
			this.pProgress.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LoseTextBoxFocus);
			// 
			// chbPauseOnEdit
			// 
			this.chbPauseOnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chbPauseOnEdit.AutoSize = true;
			this.chbPauseOnEdit.Checked = true;
			this.chbPauseOnEdit.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbPauseOnEdit.FlatAppearance.BorderSize = 0;
			this.chbPauseOnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.chbPauseOnEdit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.chbPauseOnEdit.ForeColor = System.Drawing.SystemColors.Control;
			this.chbPauseOnEdit.Location = new System.Drawing.Point(83, 379);
			this.chbPauseOnEdit.Name = "chbPauseOnEdit";
			this.chbPauseOnEdit.Size = new System.Drawing.Size(114, 19);
			this.chbPauseOnEdit.TabIndex = 10;
			this.chbPauseOnEdit.Text = "Pause On Edit";
			this.chbPauseOnEdit.UseVisualStyleBackColor = true;
			this.chbPauseOnEdit.CheckedChanged += new System.EventHandler(this.chbPauseOnEdit_CheckedChanged);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
			this.ClientSize = new System.Drawing.Size(478, 409);
			this.Controls.Add(this.pContainer);
			this.Controls.Add(this.chbPauseOnEdit);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.lblSpeed);
			this.Controls.Add(this.chbRepeat);
			this.Controls.Add(this.tbarSpeed);
			this.Controls.Add(this.btnPlayPause);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(494, 448);
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Tab Player v1.2 [Made by TominoCZ]";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
			this.Click += new System.EventHandler(this.LoseTextBoxFocus);
			this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LoseTextBoxFocus);
			this.Resize += new System.EventHandler(this.Form1_Resize);
			((System.ComponentModel.ISupportInitialize)(this.tbarSpeed)).EndInit();
			this.pTab.ResumeLayout(false);
			this.pContainer.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RichTextBox rtbTab;
		private System.Windows.Forms.Button btnPlayPause;
		private System.Windows.Forms.TrackBar tbarSpeed;
		private System.Windows.Forms.Label lblTab;
		private System.Windows.Forms.Panel pTab;
		private System.Windows.Forms.Panel pCenter;
		private System.Windows.Forms.CheckBox chbRepeat;
		private System.Windows.Forms.Timer updateTimer;
		private System.Windows.Forms.Label lblSpeed;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.Panel pContainer;
		private System.Windows.Forms.Panel pProgress;
		private System.Windows.Forms.CheckBox chbPauseOnEdit;
	}
}

