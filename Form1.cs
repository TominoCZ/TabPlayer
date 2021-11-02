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

namespace TabPlayer
{
	public partial class Form1 : Form
	{
		public static SoundPlayer SoundPlayer = new SoundPlayer();

		private Tab _tab = null;
		private DateTime _last = DateTime.MinValue;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			BassManager.Reload();

			Application.Idle += (o, _) => Tick();
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

			var speed = 1 + (tbarSpeed.Value - tbarSpeed.Maximum / 2f) / 100f *2;

			Update(delta * speed);
		}

		private void Update(double delta = 0)
		{
			if (delta < 0)
				return;

			_tab?.Update(delta);

			var progress = 0f;

			if (_tab != null)
			{
				progress = (float)Math.Min(1, _tab.Time / (float)_tab.Length);

				btnPlay.Text = _tab.Playing ? "STOP" : "PLAY";
			}
			
			var scale = lblTab.Font.Size / 16.80556;
			var offset = 5 * scale;

			lblTab.Location = new Point((int)(pTab.Size.Width / 2.0 - progress * (lblTab.Size.Width - offset * 2)), 0);

			pCenter.Size = new Size((int)1, pTab.Size.Height);
			pCenter.Location = new Point((int)(pTab.Size.Width / 2f - pCenter.Size.Width / 2f), 0);

			lblTab.Invalidate();
			pCenter.Invalidate();
		}

		private void btnPlay_Click(object sender, EventArgs e)
		{
			if (_tab != null)
			{
				var played = _tab.Playing;

				_tab.Stop();

				if (played)
				{
					return;
				}
			}

			_tab = Tab.Parse(rtbTab.Text.Split('\n'));
			_tab.Repeat = chbRepeat.Checked;
			_tab.Play();

			lblTab.Text = string.Join("\n", _tab.Data);

			if (lblTab.Text.Trim().Length < 3)
			{
				return;
			}

			var height = pTab.Size.Height;
			var width = 0;
			var line = height / (float)_tab.Data.Length / 1.2f;

			lblTab.Font = new Font(lblTab.Font.FontFamily, line, FontStyle.Regular, GraphicsUnit.Pixel);
			lblTab.AutoSize = true;

			width = lblTab.ClientSize.Width;

			lblTab.AutoSize = false;
			lblTab.Size = new Size(width, height);

			Update();
		}

		private void chbRepeat_CheckedChanged(object sender, EventArgs e)
		{
			if (_tab != null)
				_tab.Repeat = chbRepeat.Checked;
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			if ((DateTime.Now - _last).TotalMilliseconds >= 16)
			{
				Tick();
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			BassManager.Dispose();
		}
	}
}
