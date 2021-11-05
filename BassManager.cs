using System;
using Un4seen.Bass;

namespace TabPlayer
{
	static class BassManager
	{
		static BassManager()
		{
			Init();
		}

		private static void Init()
		{
			Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);

			Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_BUFFER, 250);
			Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD, 5);
		}

		public static bool CheckDevice(int streamID)
		{
			var device = Bass.BASS_ChannelGetDevice(streamID);
			var info = Bass.BASS_GetDeviceInfo(device);

			if (info != null && (!info.IsDefault || !info.IsEnabled))
			{
				return false;
			}

			return true;
		}

		public static void Stop(int stream)
		{
			Bass.BASS_StreamFree(stream);
		}

		public static void Mute(int stream)
		{
			Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, 0);
		}

		public static void Reload()
		{
			Dispose();
			Init();
		}

		public static void Dispose()
		{
			Bass.BASS_Free();
		}
	}
}