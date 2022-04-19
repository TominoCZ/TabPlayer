using System;

namespace TabPlayer
{
	public struct BufferPtr
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