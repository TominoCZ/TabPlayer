﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TabPlayer
{
	public partial class BetterLabel : Label
	{
		public Color DashColor { get; set; } = Color.FromArgb(100, 100, 100);

		public override string Text
		{
			get => _text;
			set
			{
				_text = value;

				UpdateText();
			}
		}

		private string _text = "";
		private string _dashes = "";
		private string _other = "";
		private double[] _strings = new double[1];

		private DateTime _last = DateTime.MinValue;

		public BetterLabel()
		{
			InitializeComponent();

			UpdateText();
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			var size = TextRenderer.MeasureText(e.Graphics, Text, Font, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.TextBoxControl);
			var scale = Font.Size / 16.80556;
			var offset = 5 * (float)scale;

			var height = size.Height;
			var panelSize = Parent.Size;

			var thickness = 5 * 6f / _strings.Length;

			for (int i = 0; i < _strings.Length; i++)
			{
				var val = (float)_strings[i];

				var h = val * thickness;
				var y = (i + 0.55f) / _strings.Length * height;
				var c = (int)(Parent.BackColor.R + val * 36);

				using (var sb = new SolidBrush(Color.FromArgb(c, c, c)))
				{
					var offsetStart = Math.Max(0, -Location.X - offset * 1.5f);
					var end = Math.Min(panelSize.Width, Size.Width - offsetStart - offset * 3.5f);

					var startX = offsetStart + offset * 1.5f;
					var endX = startX - end;

					var ss = startX - endX - Math.Max(0, Location.X);

					e.Graphics.FillRectangle(sb, startX, y - h / 2, ss, h);
				}
			}

			TextRenderer.DrawText(e.Graphics, _dashes, Font, Point.Empty, DashColor, Color.Transparent, TextFormatFlags.TextBoxControl);
			TextRenderer.DrawText(e.Graphics, _other, Font, Point.Empty, ForeColor, Color.Transparent, TextFormatFlags.TextBoxControl);
		}

		private double GetDelta()
		{
			var now = DateTime.Now;

			double delta = 0;

			if (_last != DateTime.MinValue)
			{
				delta = (now - _last).TotalSeconds;
			}

			_last = now;

			return delta;
		}

		public void Pluck(int strIndex, bool mute)
		{
			if (strIndex >= _strings.Length)
				return;

			_strings[strIndex] = mute ? 0 : 1;
		}

		public void Tick()
		{
			var delta = GetDelta();

			for (int i = 0; i < _strings.Length; i++)
			{
				_strings[i] = Math.Max(0, _strings[i] - delta * 2);
			}
		}

		private void UpdateText()
		{
			var text = Text;
			var dashes = "";

			for (int i = 0; i < text.Length; i++)
			{
				var c = text[i];

				if (c == '-' || c == '\n')
					dashes += c;
				else
					dashes += " ";
			}
			var other = text.Replace("-", " ");
			var lines = Math.Max(1, text.Split('\n').Length);

			_strings = new double[lines];
			_dashes = dashes;
			_other = other;
		}
	}
}
