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

			var value = newVal % 12;
			var octave = n.Octave + octaves;
			var note = Notes[value];

			return new Note
			{
				Value = value,
				Octave = octave,
				Letter = $"{note}{octave}"
			};
		}

		public static Note operator -(Note n, int offset)
		{
			return n + -offset;
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

		public string Letter;

		//public int Play(float volume = 0.25f)
		//{
		//return Form1.SoundPlayer.Play(Letter, volume);
		//}

		public static Note Parse(string note, int octave)
		{
			note = note.ToUpper();

			if (note.Length == 2)
			{
				var off = note[1] == 'B' ? -1 : (note[1] == '#' ? 1 : 0);

				var index = Array.IndexOf(Notes, note[0].ToString()) + off;
				var value = index < 0 ? Notes.Length - Math.Abs(index) % Notes.Length : index % Notes.Length;

				octave = index < 0 ? octave - 1 : octave;

				return new Note
				{
					Value = value,
					Octave = octave,
					Letter = $"{Notes[value]}{octave}"
				};
			}

			return new Note
			{
				Value = Array.IndexOf(Notes, note),
				Octave = octave,
				Letter = $"{note}{octave}"
			};
		}
	}
}
