using System;

namespace TabPlayer
{
	public class StringEventArgs : EventArgs
	{
		public int String;
		public bool Mute;

		public StringEventArgs(int index, bool mute)
		{
			String = index;
			Mute = mute;
		}
	}
}
