using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TabPlayer
{
	public partial class TuningLabel : Label
	{
		public Color NumberColor { get; set; } = Color.FromArgb(100, 100, 100);

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
		private string _numbers = "";
		private string _other = "";

		public TuningLabel()
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
			SetupGraphics(e.Graphics);

			TextRenderer.DrawText(e.Graphics, _numbers, Font, Point.Empty, NumberColor, Color.Transparent, TextFormatFlags.TextBoxControl);
			TextRenderer.DrawText(e.Graphics, _other, Font, Point.Empty, ForeColor, Color.Transparent, TextFormatFlags.TextBoxControl);
		}

		private void SetupGraphics(Graphics g)
		{
			g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
		}

		private void UpdateText()
		{
			var text = Text;
			var numbers = "";
			var other = "";
			for (int i = 0; i < text.Length; i++)
			{
				var c = text[i];

				if (c.IsNumber())
				{
					numbers += c;
					other += ' ';
				}
				else
				{
					if (c == '\n')
					{
						numbers += c;
						other += c;
					}
					else
					{
						numbers += ' ';
						other += c;
					}
				}
			}
			var lines = Math.Max(1, text.Count(c => c == '\n') + 1);

			_numbers = numbers;
			_other = other;

			Invalidate();
			Update();
		}
	}
}
