using System;
using System.Collections.Generic;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;

namespace TabPlayer
{
	public class SoundPlayer
	{
		private readonly Dictionary<string, string> _sounds = new Dictionary<string, string>();

		public SoundPlayer()
		{
		}

		public void Cache(string id, string ext = "wav")
		{
			_sounds.Add(id, $"assets/sounds/{id}.{ext}");
		}

		public int Play(string id, float volume = 1, float speed = 1)
		{
			if (_sounds.TryGetValue(id, out var sound))
			{
				var s = Bass.BASS_StreamCreateFile(sound, 0, 0, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_PRESCAN | BASSFlag.BASS_FX_FREESOURCE);//sound, 0, 0, BASSFlag.BASS_STREAM_AUTOFREE);
				
				var fxs = BassFx.BASS_FX_TempoCreate(s, BASSFlag.BASS_STREAM_PRESCAN | BASSFlag.BASS_STREAM_AUTOFREE | BASSFlag.BASS_FX_FREESOURCE | BASSFlag.BASS_MUSIC_AUTOFREE);

				Bass.BASS_ChannelSetAttribute(fxs, BASSAttribute.BASS_ATTRIB_VOL, volume);
				Bass.BASS_ChannelSetAttribute(fxs, BASSAttribute.BASS_ATTRIB_FREQ, speed * 44100);
				//var db = 20 * (float)Math.Log10(1 - speed);
				//Bass.BASS_ChannelSetAttribute(s, BASSAttribute.BASS_ATTRIB_TEMPO_PITCH, db);

				Bass.BASS_ChannelPlay(fxs, false);

				return fxs;
			}

			return -1;
		}
	}
}