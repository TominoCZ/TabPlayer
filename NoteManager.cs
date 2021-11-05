using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;

namespace TabPlayer
{
	public partial class NoteManager
	{
		private Dictionary<string, BufferPtr> _soundData = new Dictionary<string, BufferPtr>();

		public NoteManager(JSONInstrument[] loaded)
		{
			/*
			var dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("notes_nylon.json"));
			foreach (var pair in dict)
			{
				var letter = pair.Key;
				var i = pair.Value.IndexOf(',');
				var data = pair.Value.Substring(i + 1);

				//var note = Note.Parse(letter.Substring(0, letter.Length - 1), int.Parse(letter.Substring(letter.Length - 1)));

				File.WriteAllBytes("assets/sounds/guitar_nylon/" + letter + ".ogg", Convert.FromBase64String(data));
			}*/

			foreach (var instrument in loaded)
			{
				var dir = Path.Combine("assets", "sounds", instrument.ID);
				var files = Directory.GetFiles(dir);

				foreach (var file in files)
				{
					try
					{
						var letter = Path.GetFileNameWithoutExtension(file);
						var note = Note.Parse(letter);

						letter = note.Letter;

						Cache($"{letter}_{instrument.ID}", file);
					}
					catch (Exception e)
					{
						Console.WriteLine($"Failed to load file '{file}': {e}");
					}
				}
			}
		}

		private void Cache(string id, string file)
		{
			_soundData.Remove(id);

			var data = File.ReadAllBytes(file);
			var ptr = Marshal.AllocHGlobal(data.Length);

			Marshal.Copy(data, 0, ptr, data.Length);

			_soundData.Add(id, new BufferPtr(ptr, data.LongLength));
		}

		public int Play(ref Note note, JSONInstrument instrument)
		{
			var key = $"{note.Letter}_{instrument.ID}";

			if (_soundData.TryGetValue(key, out var buffer))
			{
				var stream = Bass.BASS_StreamCreateFile(buffer.Ptr, 0, buffer.Length, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_FX_FREESOURCE);
				var fxs = BassFx.BASS_FX_TempoCreate(stream, BASSFlag.BASS_STREAM_AUTOFREE | BASSFlag.BASS_MUSIC_AUTOFREE | BASSFlag.BASS_FX_FREESOURCE);

				Bass.BASS_ChannelSetAttribute(fxs, BASSAttribute.BASS_ATTRIB_VOL, 0.35f);
				Bass.BASS_ChannelPlay(fxs, false);

				return fxs;
			}

			return -1;
		}

		public void Dispose()
		{
			foreach (var buffer in _soundData.Values)
			{
				Marshal.FreeHGlobal(buffer.Ptr);
			}
		}
	}
}