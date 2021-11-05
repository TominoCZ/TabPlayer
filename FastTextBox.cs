using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TabPlayer
{
	public partial class FastTextBox : RichTextBox
	{

		public FastTextBox()
		{
			InitializeComponent();
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			SetupGraphics(e.Graphics);

			base.OnPaintBackground(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			SetupGraphics(e.Graphics);

			base.OnPaint(e);
		}

		private void SetupGraphics(Graphics g)
		{
			g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
			g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
			g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
		}
	}
}
