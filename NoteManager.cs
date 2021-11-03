using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;

namespace TabPlayer
{
	public class NoteManager
	{
		struct Buffer
		{
			public IntPtr Ptr;

			public long Length;

			public Buffer(IntPtr ptr, long length)
			{
				Ptr = ptr;
				Length = length;
			}
		}

		private static Dictionary<string, Buffer> _soundData = new Dictionary<string, Buffer>();

		static NoteManager()
		{
			/*
			var dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("notes_bass.json"));
			foreach (var pair in dict)
			{
				var letter = pair.Key;
				var i = pair.Value.IndexOf(',');
				var data = pair.Value.Substring(i + 1);

				//var note = Note.Parse(letter.Substring(0, letter.Length - 1), int.Parse(letter.Substring(letter.Length - 1)));

				File.WriteAllBytes("assets/sounds/bass/" + letter + ".ogg", Convert.FromBase64String(data));
			}*/
			var dirs = Directory.GetDirectories(Path.Combine("assets", "sounds"));

			foreach (var dir in dirs)
			{
				var info = new DirectoryInfo(dir);
				var type = info.Name.ToLower();

				var files = Directory.GetFiles(dir);

				foreach (var file in files)
				{
					try
					{
						var letter = Path.GetFileNameWithoutExtension(file).ToUpper();
						var note = Note.Parse(letter.Substring(0, letter.Length - 1), int.Parse(letter.Substring(letter.Length - 1)));

						letter = note.Letter;

						var id = $"{letter}_{type}";

						Cache(id, file);
					}
					catch (Exception e)
					{
						Console.WriteLine($"Failed to load file '{file}': {e}");
					}
				}
			}

			/*
			var files = Directory.GetFiles("assets/sounds");
			foreach (var file in files)
			{
				Form1.SoundPlayer.Cache(Path.GetFileNameWithoutExtension(file), "ogg");
			}*/
		}

		private static void Cache(string id, string file)
		{
			_soundData.Remove(id);

			var data = File.ReadAllBytes(file);
			var ptr = Marshal.AllocHGlobal(data.Length);

			Marshal.Copy(data, 0, ptr, data.Length);

			_soundData.Add(id, new Buffer(ptr, data.LongLength));
		}

		public int Play(ref Note note, Instrument instrument)
		{
			var key = $"{note.Letter}_{instrument.ToString().ToLower()}";

			if (_soundData.TryGetValue(key, out var buffer))
			{
				var stream = Bass.BASS_StreamCreateFile(buffer.Ptr, 0, buffer.Length, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_FX_FREESOURCE);
				var fxs = BassFx.BASS_FX_TempoCreate(stream, BASSFlag.BASS_STREAM_AUTOFREE | BASSFlag.BASS_FX_FREESOURCE | BASSFlag.BASS_MUSIC_AUTOFREE);

				Bass.BASS_ChannelSetAttribute(fxs, BASSAttribute.BASS_ATTRIB_VOL, 0.25f);
				Bass.BASS_ChannelPlay(fxs, false);

				return fxs;
			}

			return -1;
		}

		public static void Dispose()
		{
			foreach (var buffer in _soundData.Values)
			{
				Marshal.FreeHGlobal(buffer.Ptr);
			}
		}
	}
}