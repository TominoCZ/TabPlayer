using System;
using System.Collections.Generic;
using System.IO;

namespace TabPlayer
{
	public class Tab
	{
		public string[] Data;
		public Note[] Tuning;
		public int[] RingingStrings;

		public double Time;
		public int Index;
		public int Length;
		public bool Playing;
		public bool Paused;
		public bool Repeat;

		public void Play()
		{
			Time = 0;
			Index = 0;

			Resume();
		}

		public void Pause()
		{
			Playing = false;
			Paused = true;
		}

		public void Resume()
		{
			Playing = true;
			Paused = false;
		}

		public void Stop()
		{
			Time = 0;
			Index = 0;
			Playing = false;
			Paused = false;
		}

		private int[] GetNotes(int index)
		{
			var notes = new int[Data.Length];

			for (int i = 0; i < Data.Length; i++)
			{
				var line = Data[i];

				notes[i] = -1;

				if (index >= line.Length)
					continue;

				if (char.ToUpper(line[index]) == 'X')
				{
					notes[i] = -2;
				}
				else if (int.TryParse(line[index].ToString(), out var num))
				{
					if (index > 0
						   && int.TryParse(line[index - 1].ToString(), out _)
						   && int.TryParse(line[index].ToString(), out _)
						   && int.TryParse(line[index - 1].ToString() + line[index], out _))
					{
						continue;
					}
					else if (index < line.Length - 1
					   && int.TryParse(line[index + 1].ToString(), out _)
					   && int.TryParse(line[index].ToString(), out _)
					   && int.TryParse(line[index].ToString() + line[index + 1], out var num1))
					{
						num = num1;
					}
					else if (int.TryParse(line[index].ToString(), out var num2))
					{
						num = num2;
					}

					notes[i] = num;
				}
			}

			return notes;
		}

		public void Update(double delta)
		{
			if (Playing && delta >= 0)
			{
				Time += delta * 10;
			}

			var newIndex = (int)Math.Min(Length, Math.Floor(Time + 0.5));

			if (delta >= 0)
			{
				for (int i = Index; i < newIndex; i++)
				{
					var notes = GetNotes(i);

					for (int strIndex = 0; strIndex < notes.Length; strIndex++)
					{
						var offset = notes[strIndex];
						if (offset > -1)
						{
							var note = Tuning[strIndex] + offset;
							var s = note.Play();

							if (strIndex < RingingStrings.Length)
							{
								BassManager.Mute(RingingStrings[strIndex]);

								RingingStrings[strIndex] = s;
							}
						}
						else if (offset == -2)
						{
							if (strIndex < RingingStrings.Length)
							{
								BassManager.Mute(RingingStrings[strIndex]);
							}
						}
					}
				}
			}

			var over = Time - Length;

			if (over >= 0 && Playing)
			{
				if (Repeat)
				{
					Index = 0;
					Time = over;

					return;
				}
				else
				{
					Playing = false;
					Time = Length;
					Index = Length;
				}
			}

			Index = newIndex;
		}

		public static Tab Parse(string[] lines)
		{
			var started = false;
			var stringsSet = false;
			var strings = 0;
			var stringIndex = 0;

			var standardTuning = new[] {
				Note.FromNote("E", 4),
				Note.FromNote("B", 3),
				Note.FromNote("G", 3),
				Note.FromNote("D", 3),
				Note.FromNote("A", 2),
				Note.FromNote("E", 2)
			};
			var tuning = new List<Note>();

			List<string> tab = new List<string>();

			for (int i = 0; i < lines.Length; i++)
			{
				var line = lines[i].Trim().Replace(" ", "");

				if (line.Contains("-") || line.Contains("|-") || line.Contains("-|") || line.Contains("-|-"))
				{
					started = true;

					var firstPipe = line.IndexOf('|');

					var note = standardTuning[Math.Min(standardTuning.Length - 1, stringIndex)];

					if (firstPipe > -1 && firstPipe <= 2)
					{
						if (firstPipe > 0)
						{
							note = Note.FromNote(line.Substring(0, firstPipe), standardTuning[Math.Min(standardTuning.Length - 1, stringIndex)].Octave);
						}

						line = line.Substring(firstPipe + 1, line.Length - firstPipe - 1);
					}

					if (!stringsSet)
					{
						tuning.Add(note);

						strings++;
					}

					if (stringIndex >= tab.Count)
					{
						tab.Add(line);
					}
					else
					{
						tab[stringIndex] += line;
					}

					stringIndex++;

					if (stringsSet)
						stringIndex %= strings;
				}
				else if (started)
				{
					started = false;
					stringsSet = true;
					stringIndex = 0;
				}
			}

			var ringing = new int[strings];
			var length = 0;

			for (int i = 0; i < tab.Count; i++)
			{
				var line = tab[i];//.Replace("-|-", "");
								  //line = line.Replace("-|", "-");
								  //line = line.Replace("|-", "-");
				line = line.Replace("|", "");
				//line = "|-" + line + "-|";

				tab[i] = line;

				ringing[i] = -1;

				length = Math.Max(length, line.Length);
			}

			return new Tab()
			{
				Data = tab.ToArray(),
				Tuning = tuning.ToArray(),
				RingingStrings = ringing,
				Length = length
			};
		}
	}
}
