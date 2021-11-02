using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using TabPlayer.Properties;

namespace TabPlayer
{
	public partial class Form1 : Form
	{
		public static SoundPlayer SoundPlayer = new SoundPlayer();

		private Tab _tab = null;
		private DateTime _last = DateTime.MinValue;
		private Point _mouseDownPoint;

		private double _mouseDownTime;
		private double _tabOffset = 0;
		private double _tabWidth = 100;

		private bool _spaceDown;
		private bool _mouseDown;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Settings.Default.Reload();

			BassManager.Reload();

			Application.Idle += (o, _) => Tick();

			SetTab();

			chbRepeat.Checked = Settings.Default.Repeat;
			chbPauseOnEdit.Checked = Settings.Default.PauseOnEdit;
		}

		private void Tick()
		{
			var now = DateTime.Now;

			double delta = 0;

			if (_last != DateTime.MinValue)
			{
				delta = (now - _last).TotalSeconds;
			}

			_last = now;
			
			Update(delta * tbarSpeed.Value / 100);
		}

		private void Update(double delta = 0)
		{
			if (delta < 0)
				return;

			if (Keyboard.IsKeyDown(Key.Space) && !btnPlayPause.Focused && ActiveControl != rtbTab && ContainsFocus)
			{
				if (!_spaceDown)
				{
					if (_tab.Playing)
						_tab?.Pause();
					else
						_tab?.Resume();
				}

				_spaceDown = true;
			}
			else
			{
				_spaceDown = false;
			}

			if (_mouseDown)
			{
				var mouse = PointToClient(System.Windows.Forms.Cursor.Position);
				var drag = _mouseDownPoint.X - mouse.X;
				var offset = drag / _tabWidth * _tab.Length;

				var time = _mouseDownTime + offset;

				_tab.Time = Math.Max(Math.Min(_tab.Length, time), 0);

				_tab.Update(-1);
			}
			else
			{
				_tab?.Update(delta);
			}

			var progress = 0f;

			if (_tab != null)
			{
				progress = (float)Math.Min(1, _tab.Time / (float)_tab.Length);

				btnPlayPause.Text = !_tab.Playing || _tab.Paused ? "PLAY" : "PAUSE";
			}

			lblTab.Location = new Point((int)(pTab.Size.Width / 2.0 - progress * _tabWidth - _tabOffset + 1), 0);

			pProgress.Size = new Size((int)(pTab.Size.Width * progress), pProgress.Size.Height);

			pCenter.Size = new Size((int)1, pTab.Size.Height);
			pCenter.Location = new Point((int)(pTab.Size.Width / 2f - pCenter.Size.Width / 2f), 0);

			lblTab.Invalidate();
			pCenter.Invalidate();
		}

		private void SetTab()
		{
			var tab = Tab.Parse(rtbTab.Text.Split('\n'));
			tab.RingingStrings = _tab?.RingingStrings ?? tab.RingingStrings;
			tab.Index = _tab?.Index ?? tab.Index;
			tab.Time = _tab?.Time ?? tab.Time;
			tab.Playing = _tab?.Playing ?? tab.Playing;
			tab.Paused = _tab?.Paused ?? tab.Paused;
			tab.Repeat = chbRepeat.Checked;

			if (chbPauseOnEdit.Checked)
				tab.Pause();

			lblTab.Text = string.Join("\n", tab.Data);

			if (lblTab.Text.Trim().Length < 3)
			{
				return;
			}

			var height = pTab.Size.Height;
			var line = height / (float)tab.Data.Length / 1.2f;

			lblTab.Font = new Font(lblTab.Font.FontFamily, line, FontStyle.Regular, GraphicsUnit.Pixel);
			lblTab.AutoSize = true;

			var width = lblTab.ClientSize.Width;

			lblTab.AutoSize = false;
			lblTab.Size = new Size(width, height);

			var scale = lblTab.Font.Size / 16.80556;
			var offset = 5 * scale;

			_tabOffset = offset;

			_tabWidth = lblTab.Size.Width - offset * 2;
			_tab = tab;

			Update();
		}

		private void SaveSettings()
		{
			Settings.Default.Repeat = chbRepeat.Checked;
			Settings.Default.PauseOnEdit = chbPauseOnEdit.Checked;

			Settings.Default.Save();
		}

		private void rtbTab_TextChanged(object sender, EventArgs e)
		{
			SetTab();
		}

		private void chbRepeat_CheckedChanged(object sender, EventArgs e)
		{
			if (_tab != null)
				_tab.Repeat = chbRepeat.Checked;

			ActiveControl = lblTab;

			SaveSettings();
		}

		private void chbPauseOnEdit_CheckedChanged(object sender, EventArgs e)
		{
			ActiveControl = lblTab;

			SaveSettings();
		}

		private void tbarSpeed_Scroll(object sender, EventArgs e)
		{
			lblSpeed.Text = $"Speed: {(int)tbarSpeed.Value}%";
		}

		private void lblTab_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (_tab == null)
				return;

			_mouseDownPoint = PointToClient(System.Windows.Forms.Cursor.Position);
			_mouseDownTime = _tab?.Time ?? 0;

			_mouseDown = true;
		}

		private void lblTab_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			_mouseDown = false;
		}

		private void tbarSpeed_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			ActiveControl = lblTab;
		}

		private void LoseTextBoxFocus(object sender, EventArgs e)
		{
			if (ActiveControl == rtbTab)
			{
				ActiveControl = lblTab;
			}
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			_tab?.Stop();

			ActiveControl = lblTab;
		}

		private void btnPlayPause_Click(object sender, EventArgs e)
		{
			if (_tab != null)
			{
				if (_tab.Paused || !_tab.Playing)
					_tab.Resume();
				else
					_tab.Pause();
			}

			ActiveControl = lblTab;
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			if ((DateTime.Now - _last).TotalMilliseconds >= 16)
			{
				Tick();
			}
		}

		private void Form1_Resize(object sender, EventArgs e)
		{
			Update();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			BassManager.Dispose();
		}
	}
}
