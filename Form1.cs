using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using TabPlayer.Properties;

namespace TabPlayer
{
	public partial class Form1 : Form
	{
		public static Form1 Instance;

		public NoteManager NoteManager;

		private Tab _tab = null;
		private DateTime _last = DateTime.MinValue;
		private Point _mouseDownPoint;

		private double _mouseDownTime;
		private double _tabOffset = 0;
		private double _tabWidth = 100;

		private int _dashPerSecond = 10;

		private bool _spaceDown;
		private bool _mouseDown;

		private bool _loaded = false;

		public Form1()
		{
			Instance = this;

			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Application.Idle += OnIdle;

			Settings.Default.Reload();

			BassManager.Reload();

			LoadInstruments();

			rtbTab.AllowDrop = true;
			rtbTab.DragDrop += rtbTab_DragDrop;

			rtbTab.Text = Settings.Default.Tab;
			Size = Settings.Default.Size;

			SetTab();

			if (_tab is Tab tab)
			{
				tab.Time = Math.Min(tab.Length, Settings.Default.Time);
				tab.Update(true);
			}

			chbAutoMute.Checked = Settings.Default.AutoMute;
			chbRepeat.Checked = Settings.Default.Repeat;
			chbMerge.Checked = Settings.Default.Merge;
			chbPauseOnEdit.Checked = Settings.Default.PauseOnEdit;
			tbarSpeed.Value = Math.Max(tbarSpeed.Minimum, Math.Min(tbarSpeed.Maximum, Settings.Default.Speed));
			tbarSpeed_ValueChanged(null, null);

			_loaded = true;
		}

		private void OnIdle(object sender, EventArgs e)
		{
			_last = DateTime.Now;

			UpdateTab();
		}

		private void LoadInstruments()
		{
			var config = "instruments.json";

			var loaded = new JSONInstrument[] { };

			try
			{
				var json = File.ReadAllText(config);
				loaded = JsonConvert.DeserializeObject<JSONInstrument[]>(json);

				foreach (var instrument in loaded)
				{
					cbInstrument.Items.Add(instrument);
				}
			}
			catch
			{
				loaded = new JSONInstrument[]
				{
					new JSONInstrument
					{
						ID = "guitar",
						Name = "Guitar",
						Tuning = new[]{ "E4", "B3", "G3", "D3", "A2", "E2" }
					},
					new JSONInstrument
					{
						ID = "guitar_nylon",
						Name = "Nylon Guitar",
						Tuning = new[]{ "E4", "B3", "G3", "D3", "A2", "E2" }
					},
					new JSONInstrument
					{
						ID = "guitar_nylon",
						Name = "Ukulele",
						Tuning = new[]{ "A4", "E4", "C4", "G4" }
					},
					new JSONInstrument
					{
						ID = "bass",
						Name = "Bass",
						Tuning = new[]{ "G2", "D2", "A1", "E1" }
					}
				};

				cbInstrument.Items.AddRange(loaded);

				var json = JsonConvert.SerializeObject(loaded, Formatting.Indented);

				try
				{
					File.WriteAllText(config, json);
				}
				catch
				{

				}
			}

			NoteManager = new NoteManager(loaded);

			var ins = Settings.Default.Instrument;
			if (ins < 0 || ins >= cbInstrument.Items.Count)
				ins = 0;

			if (loaded.Length > 0)
				cbInstrument.SelectedIndex = ins;
		}

		private void UpdateTab(bool skipStep = false)
		{
			if (skipStep)
				return;

			var captureInput = _tab != null && !btnPlayPause.Focused && ActiveControl != rtbTab && ContainsFocus;

			if (captureInput && (Keyboard.IsKeyDown(Key.R) || Keyboard.IsKeyDown(Key.S)))
			{
				_tab?.Stop();
			}

			if (captureInput && Keyboard.IsKeyDown(Key.Space) && _tab != null)
			{
				if (!_spaceDown && _tab != null)
				{
					if (_tab.Playing)
					{
						_tab.Pause();
					}
					else
					{
						if (_tab.Time == _tab.Length)
						{
							_tab.Play();
						}
						else
						{
							_tab.Resume();
						}
					}
				}

				_spaceDown = true;
			}
			else
			{
				_spaceDown = false;
			}

			if (_mouseDown && _tab != null)
			{
				var mouse = PointToClient(System.Windows.Forms.Cursor.Position);
				var drag = _mouseDownPoint.X - mouse.X;
				var offset = drag / _tabWidth * _tab.Length;

				var time = _mouseDownTime + offset;

				_tab.Time = Math.Max(Math.Min(_tab.Length, time), 0);
				_tab.Update(true);
			}
			else
			{
				_tab?.Update();
			}

			var progress = 0.0;

			if (_tab != null)
			{
				progress = Math.Max(0, _tab.Time - 0.5) / _tab.Length;

				btnPlayPause.Text = !_tab.Playing || _tab.Paused ? "PLAY" : "PAUSE";

				lblTab.Tick();
			}

			lblTab.Location = new Point((int)(pTab.Size.Width / 2.0 - progress * _tabWidth - _tabOffset + 1), 0);
			lblTuning.Location = new Point((int)(lblTab.Location.X - lblTuning.ClientSize.Width + _tabOffset * 1.45), lblTab.Location.Y);

			pProgress.Size = new Size((int)(pTab.Size.Width * (_tab?.Progress ?? 0)), pProgress.Size.Height);

			pCenter.Size = new Size(1, pTab.Size.Height);
			pCenter.Location = new Point((int)(pTab.Size.Width / 2f - pCenter.Size.Width / 2f), 0);

			var rect = pTab.ClientRectangle;
			rect.Offset(-lblTab.Location.X, 0);

			lblTab.Invalidate(rect);
		}

		private Tab SetTab(bool adjustForStops = false)
		{
			if (cbInstrument.Items.Count == 0)
				return null;

			if (!Tab.TryParse(rtbTab.Text.Split('\n'), chbMerge.Checked, (JSONInstrument)cbInstrument.Items[cbInstrument.SelectedIndex], out var tab))
				return null;

			tab.OnPluck += OnPluck;
			tab.DashPerScond = _tab?.DashPerScond ?? _dashPerSecond * tbarSpeed.Value / 100.0;
			tab.RingingStrings = _tab?.RingingStrings ?? tab.RingingStrings;
			tab.Index = _tab?.Index ?? tab.Index;
			tab.Time = _tab?.Time ?? tab.Time;
			tab.Playing = _tab?.Playing ?? tab.Playing;
			tab.Paused = _tab?.Paused ?? tab.Paused;
			tab.Repeat = chbRepeat.Checked;
			tab.AutoMute = chbAutoMute.Checked;

			if (adjustForStops)
			{
				var skips = tab.CountSkips();
				var skipsLast = _tab?.CountSkips() ?? skips;

				var diff = skips - skipsLast;

				tab.Index += diff;
				tab.Time += diff;
			}

			var content = string.Join("\n", tab.Data).Trim();
			if (content.Length == 0)
			{
				return null;
			}

			lblTab.Text = content;
			lblTuning.Text = string.Join("\n", tab.Tuning.Select(n => n.Letter));

			var width = lblTab.ClientSize.Width;
			var height = pTab.Size.Height;

			var line = height / (float)tab.Data.Length / 1.2f;
			var font = new Font(lblTab.Font.FontFamily, line, FontStyle.Regular, GraphicsUnit.Pixel);

			var tuningWidth = 1;

			using (var bmp = new Bitmap(1, 1))
			{
				using (var g = Graphics.FromImage(bmp))
				{
					var size = TextRenderer.MeasureText(g, content, font, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.TextBoxControl);
					width = size.Width;

					size = TextRenderer.MeasureText(g, lblTuning.Text, font, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.TextBoxControl);
					tuningWidth = size.Width;
				}
			}

			lblTab.Font = font;
			lblTab.Size = new Size(width, height);

			lblTuning.Font = lblTab.Font;
			lblTuning.Size = new Size(tuningWidth, height);

			lblTuning.BackColor = lblTab.BackColor;
			lblTuning.ForeColor = lblTab.ForeColor;

			var scale = lblTab.Font.Size / 16.80556;
			var offset = 5 * scale;

			_tabOffset = offset;

			_tabWidth = lblTab.Size.Width - offset * 2;
			_tab = tab;

			UpdateTab();

			Settings.Default.Tab = rtbTab.Text;

			SaveSettings();

			return tab;
		}

		private void SaveSettings()
		{
			if (!_loaded)
				return;

			Settings.Default.Time = _tab?.Time ?? 0;
			Settings.Default.Repeat = chbRepeat.Checked;
			Settings.Default.PauseOnEdit = chbPauseOnEdit.Checked;
			Settings.Default.AutoMute = chbAutoMute.Checked;
			Settings.Default.Merge = chbMerge.Checked;
			Settings.Default.Speed = tbarSpeed.Value;
			Settings.Default.Size = Size;

			Settings.Default.Instrument = cbInstrument.SelectedIndex;

			Settings.Default.Save();
		}

		private void OnPluck(object _, StringEventArgs e)
		{
			lblTab.Pluck(e.String, e.Mute);
		}

		private void rtbTab_DragDrop(object o, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				var files = (string[])e.Data.GetData(DataFormats.FileDrop);
				var file = files[0];

				if (File.Exists(file))
				{
					try
					{
						rtbTab.Text = File.ReadAllText(file);

						var old = _tab;
						var tab = SetTab();

						if (tab != null)
						{
							if (_tab != null && _tab.Playing)
							{
								tab.Play();
							}
							else
							{
								tab.Stop();
							}
						}
					}
					catch
					{

					}
				}
			}
		}

		private void rtbTab_TextChanged(object sender, EventArgs e)
		{
			var tab = SetTab();

			if (tab != null)
			{
				if (chbPauseOnEdit.Checked)
				{
					tab.Pause();
				}
			}

			SaveSettings();
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

		private void chbAutoMute_CheckedChanged(object sender, EventArgs e)
		{
			if (_tab != null)
				_tab.AutoMute = chbAutoMute.Checked;

			ActiveControl = lblTab;

			SaveSettings();
		}

		private void chbMerge_CheckedChanged(object sender, EventArgs e)
		{
			SetTab(true);

			ActiveControl = lblTab;

			SaveSettings();
		}

		private void tbarSpeed_ValueChanged(object sender, EventArgs e)
		{
			var dps = _dashPerSecond * tbarSpeed.Value / 100.0;

			if (_tab != null)
				_tab.DashPerScond = dps;

			lblSpeed.Text = $"Speed: {tbarSpeed.Value}% ({dps:F1} dashes/s)";

			SaveSettings();
		}

		private void cbInstrument_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loaded)
			{
				SetTab();
			}

			SaveSettings();
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
				if (!_tab.Playing && _tab.Index == _tab.Length)
					_tab.Play();
				else
				{
					if (_tab.Paused || !_tab.Playing)
						_tab.Resume();
					else
						_tab.Pause();
				}
			}

			ActiveControl = lblTab;
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			if ((DateTime.Now - _last).TotalMilliseconds >= 16)
			{
				OnIdle(null, null);
			}
		}

		private void Form1_Resize(object sender, EventArgs e)
		{
			UpdateTab();
		}

		private void Form1_ResizeEnd(object sender, EventArgs e)
		{
			SaveSettings();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			NoteManager.Dispose();
			BassManager.Dispose();

			lblTab.Dispose();

			Application.Idle -= OnIdle;

			SaveSettings();
		}

		private void LoseTextBoxFocus(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			ActiveControl = lblTab;
		}
	}
}
