using NAudio.Vorbis;
using NAudio.Wave;
using NVorbis;
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
        private Dictionary<string, SoundData> _soundData = new Dictionary<string, SoundData>();

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
                        // Console.WriteLine($"Failed to load file '{file}': {e}");
                    }
                }
            }
        }

        private void Cache(string id, string file)
        {
            _soundData.Remove(id);

            var data = File.ReadAllBytes(file);
            var ptr = Marshal.AllocHGlobal(data.Length);
            var bptr = new BufferPtr(ptr, data.LongLength);

            Marshal.Copy(data, 0, ptr, data.Length);

            var stream = Bass.BASS_StreamCreateFile(ptr, 0, data.Length, BASSFlag.BASS_STREAM_PRESCAN | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_FX_FREESOURCE | BASSFlag.BASS_SAMPLE_8BITS);

            var length = (int)Bass.BASS_ChannelGetLength(stream);
            var buffer = new byte[length];
            var sampleRate = 0f;
            Bass.BASS_ChannelGetData(stream, buffer, length);
            Bass.BASS_ChannelGetAttribute(stream, BASSAttribute.BASS_ATTRIB_FREQ, ref sampleRate);
            Bass.BASS_StreamFree(stream);

            _soundData.Add(id, new SoundData { Buffer = bptr, Data = buffer, WaveFormat = new WaveFormat((int)sampleRate, 16, 2) });
        }

        public int Play(ref Note note, JSONInstrument instrument)
        {
            var key = $"{note.Letter}_{instrument.ID}";

            if (_soundData.TryGetValue(key, out var data))
            {
                var buffer = data.Buffer;
                var stream = Bass.BASS_StreamCreateFile(buffer.Ptr, 0, buffer.Length, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_FX_FREESOURCE);
                var fxs = BassFx.BASS_FX_TempoCreate(stream, BASSFlag.BASS_STREAM_AUTOFREE | BASSFlag.BASS_MUSIC_AUTOFREE | BASSFlag.BASS_FX_FREESOURCE);

                Bass.BASS_ChannelSetAttribute(fxs, BASSAttribute.BASS_ATTRIB_VOL, 0.35f);
                Bass.BASS_ChannelPlay(fxs, false);

                return fxs;
            }

            return -1;
        }

        public SoundData GetRaw(ref Note note, JSONInstrument instrument)
        {
            var key = $"{note.Letter}_{instrument.ID}";

            if (_soundData.TryGetValue(key, out var data))
            {
                /*byte[] bfr = new byte[data.Buffer.Length];
				Marshal.Copy(data.Buffer.Ptr, bfr, 0, (int)bfr.Length);

				return bfr;*/

                return data;
            }

            return null;
            //return //new byte[] { };
        }

        public void Dispose()
        {
            foreach (var data in _soundData.Values)
            {
                Marshal.FreeHGlobal(data.Buffer.Ptr);
            }
        }
    }

    public class SoundData
    {
        public WaveFormat WaveFormat;

        public BufferPtr Buffer;

        public byte[] Data;
    }
}