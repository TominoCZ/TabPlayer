using System;

namespace TabPlayer
{
	public struct Note
	{
		public static Note operator +(Note n, int offset)
		{
			var newVal = n.Value + offset;
			var octaves = newVal / 12;

			var value = newVal < 12 ? newVal : newVal % 12;
			var octave = n.Octave + octaves;
			var note = Notes[value];

			n.Value = value;
			n.Octave = octave;
			n.Letter = $"{note}{octave}";

			return n;
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

		public static bool IsNote(string note)
		{
			note = note.ToUpper();

			return note.Length > 0 && (note[0] == 'H' || Array.IndexOf(Notes, $"{note[0]}") > -1);
		}

		public static bool GetOctave(string note, out int octave)
		{
			var combined = "";
			for (int i = note.Length - 1; i >= 0; i--)
			{
				var c = note[i];

				if (!c.IsNumber())
					break;

				combined = c + combined;
			}

			return int.TryParse(combined, out octave);
		}

		public static Note Parse(string note)
		{
			var whole = note.ToUpper();
			var n = whole.Substring(0, whole.Length - 1);
			if (n.Length >= 2)
			{
				if (n[1] == 'B' || n[1] == '#')
				{
					n = n.Substring(0, 2);
				}
				else
				{
					n = $"{n[0]}";
				}
			}

			GetOctave(whole, out var o);

			return Parse(n, o);
		}

		public static Note Parse(string note, int octave)
		{
			note = note.ToUpper();

			if (note.Length >= 2 && (note[1] == 'B' || note[1] == '#'))
			{
				note = note.Substring(0, 2);
			}
			else
			{
				note = $"{note[0]}";
			}

			var origNote = note;
			if (origNote[0] == 'H')
			{
				var str = "B";
				if (note.Length >= 2)
				{
					str += note[1];
				}
				note = str;
			}

			if (note.Length >= 2)
			{
				var off = note[1] == 'B' ? -1 : (note[1] == '#' ? 1 : 0);
				var index = Array.IndexOf(Notes, $"{note[0]}") + off;
				var value = index < 0 ? Notes.Length - Math.Abs(index) % Notes.Length : index % Notes.Length;

				octave = index < 0 ? octave - 1 : octave;

				note = Notes[value];

				if (origNote == "H")
				{
					note = "H";
				}

				return new Note()
				{
					Value = value,
					Octave = octave,
					Letter = $"{note}{octave}"
				};
			}

			return new Note
			{
				Value = Array.IndexOf(Notes, note),
				Octave = octave,
				Letter = $"{(origNote == "H" ? origNote : note)}{octave}"
			};
		}
	}
}
