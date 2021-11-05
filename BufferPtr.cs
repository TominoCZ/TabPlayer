using System;

namespace TabPlayer
{
	public partial class NoteManager
	{
		struct BufferPtr
		{
			public IntPtr Ptr;

			public long Length;

			public BufferPtr(IntPtr ptr, long length)
			{
				Ptr = ptr;
				Length = length;
			}
		}
	}
}