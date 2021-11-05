using System;
using System.Threading;
using System.Windows.Forms;

namespace TabPlayer
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
			/*
			f.Load += (o, e) =>
			{
				new Thread(() =>
				{
					var me = (MethodInvoker)(() =>
					{
						if (f.IsHandleCreated && !f.Disposing && !f.IsDisposed && f.Visible)
						{
							f.OnIdle();
						}
					});

					while (f.IsHandleCreated && !f.Disposing && !f.IsDisposed && f.Visible)
					{
						f.BeginInvoke(me);
						Thread.Sleep(5);
					}
				}).Start();
			};*/
		}
	}
}