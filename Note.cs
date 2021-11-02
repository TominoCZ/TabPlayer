using System;
using System.IO;

namespace TabPlayer
{
	public struct Note
	{
		public static Note operator +(Note n, int offset)
		{
			var newVal = n.Value + offset;
			var octaves = (int)Math.Floor(newVal / 12.0);

			return new Note
			{
				Value = newVal % 12,
				Octave = n.Octave + octaves
			};
		}

		public static Note operator -(Note n, int offset)
		{
			return n + -offset;
		}

		public static bool operator >(Note n1, Note n2)
		{
			return n1.Octave > n2.Octave || n1.Octave == n2.Octave && n1.Value > n2.Value;
		}

		public static bool operator <(Note n1, Note n2)
		{
			return n1.Octave < n2.Octave || n1.Octave == n2.Octave && n1.Value < n2.Value;
		}

		public static string[] Notes = new[]
		{
			"C", "C#", "D",
			"D#", "E", "F",
			"F#", "G", "G#",
			"A", "A#", "B",
		};

		public int Value;
		public int Octave;

		public string Letter => Notes[Value] + Octave;

		public int Play(float volume = 1)
		{
			return Form1.SoundPlayer.Play(Letter, volume);
		}

		public static Note FromNote(string note, int octave)
		{
			note = note.ToUpper();

			if (note.Length == 2 && note[1] == 'B')
			{
				var index = Array.IndexOf(Notes, note[0].ToString()) - 1;
				var value = index % Notes.Length;

				return new Note
				{
					Value = value,
					Octave = index < 0 ? octave - 1 : octave
				};
			}

			return new Note
			{
				Value = Array.IndexOf(Notes, note),
				Octave = octave
			};
		}

		static Note()
		{
			var files = Directory.GetFiles("assets/sounds");
			foreach (var file in files)
			{
				Form1.SoundPlayer.Cache(Path.GetFileNameWithoutExtension(file), "ogg");
			}
		}
	}
}
