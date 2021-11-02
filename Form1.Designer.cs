
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
			this.btnPlay = new System.Windows.Forms.Button();
			this.tbarSpeed = new System.Windows.Forms.TrackBar();
			this.lblTab = new System.Windows.Forms.Label();
			this.pTab = new System.Windows.Forms.Panel();
			this.pCenter = new System.Windows.Forms.Panel();
			this.chbRepeat = new System.Windows.Forms.CheckBox();
			this.updateTimer = new System.Windows.Forms.Timer(this.components);
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.tbarSpeed)).BeginInit();
			this.pTab.SuspendLayout();
			this.SuspendLayout();
			// 
			// rtbTab
			// 
			this.rtbTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rtbTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
			this.rtbTab.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rtbTab.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.rtbTab.ForeColor = System.Drawing.SystemColors.Menu;
			this.rtbTab.Location = new System.Drawing.Point(12, 15);
			this.rtbTab.Name = "rtbTab";
			this.rtbTab.Size = new System.Drawing.Size(484, 193);
			this.rtbTab.TabIndex = 0;
			this.rtbTab.Text = resources.GetString("rtbTab.Text");
			this.rtbTab.WordWrap = false;
			// 
			// btnPlay
			// 
			this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnPlay.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.btnPlay.Location = new System.Drawing.Point(12, 371);
			this.btnPlay.Name = "btnPlay";
			this.btnPlay.Size = new System.Drawing.Size(114, 45);
			this.btnPlay.TabIndex = 1;
			this.btnPlay.Text = "PLAY";
			this.btnPlay.UseVisualStyleBackColor = true;
			this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
			// 
			// tbarSpeed
			// 
			this.tbarSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbarSpeed.LargeChange = 10;
			this.tbarSpeed.Location = new System.Drawing.Point(132, 371);
			this.tbarSpeed.Maximum = 100;
			this.tbarSpeed.Name = "tbarSpeed";
			this.tbarSpeed.Size = new System.Drawing.Size(364, 45);
			this.tbarSpeed.SmallChange = 10;
			this.tbarSpeed.TabIndex = 2;
			this.tbarSpeed.TickFrequency = 10;
			this.tbarSpeed.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.tbarSpeed.Value = 50;
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
			// 
			// pTab
			// 
			this.pTab.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
			this.pTab.Controls.Add(this.pCenter);
			this.pTab.Controls.Add(this.lblTab);
			this.pTab.Location = new System.Drawing.Point(12, 214);
			this.pTab.Name = "pTab";
			this.pTab.Size = new System.Drawing.Size(484, 121);
			this.pTab.TabIndex = 6;
			// 
			// pCenter
			// 
			this.pCenter.BackColor = System.Drawing.Color.Yellow;
			this.pCenter.Location = new System.Drawing.Point(309, 0);
			this.pCenter.Name = "pCenter";
			this.pCenter.Size = new System.Drawing.Size(2, 122);
			this.pCenter.TabIndex = 6;
			// 
			// chbRepeat
			// 
			this.chbRepeat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chbRepeat.AutoSize = true;
			this.chbRepeat.Checked = true;
			this.chbRepeat.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbRepeat.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.chbRepeat.Location = new System.Drawing.Point(16, 347);
			this.chbRepeat.Name = "chbRepeat";
			this.chbRepeat.Size = new System.Drawing.Size(68, 18);
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
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.label1.Location = new System.Drawing.Point(129, 348);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(49, 14);
			this.label1.TabIndex = 8;
			this.label1.Text = "Speed:";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(508, 428);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.chbRepeat);
			this.Controls.Add(this.pTab);
			this.Controls.Add(this.tbarSpeed);
			this.Controls.Add(this.btnPlay);
			this.Controls.Add(this.rtbTab);
			this.Name = "Form1";
			this.Text = "Tab Player";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.tbarSpeed)).EndInit();
			this.pTab.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RichTextBox rtbTab;
		private System.Windows.Forms.Button btnPlay;
		private System.Windows.Forms.TrackBar tbarSpeed;
		private System.Windows.Forms.Label lblTab;
		private System.Windows.Forms.Panel pTab;
		private System.Windows.Forms.Panel pCenter;
		private System.Windows.Forms.CheckBox chbRepeat;
		private System.Windows.Forms.Timer updateTimer;
		private System.Windows.Forms.Label label1;
	}
}

