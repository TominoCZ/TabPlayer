using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TabPlayer
{
	public class StringEventArgs : EventArgs
	{
		public int String;
		public bool Mute;

		public StringEventArgs(int index, bool mute)
		{
			String = index;
			Mute = mute;
		}
	}

	public class Tab
	{
		/*
		public static Dictionary<JSONInstrument, Note[]> _standardTuning = new Dictionary<JSONInstrument, Note[]>()
		{
			{
				Instrument.Guitar,
				new[]
				{
					Note.Parse("E", 4),
					Note.Parse("B", 3),
					Note.Parse("G", 3),
					Note.Parse("D", 3),
					Note.Parse("A", 2),
					Note.Parse("E", 2)
				}
			},
			{
				Instrument.Bass,
				new[]
				{
					Note.Parse("G", 2),
					Note.Parse("D", 2),
					Note.Parse("A", 1),
					Note.Parse("E", 1)
				}
			}
		};*/

		public JSONInstrument Instrument;

		public event EventHandler<StringEventArgs> OnPluck;

		public string[] Data;
		public Note[] Tuning;
		public int[] RingingStrings;

		public double Time;
		public double DashPerScond;
		public int Index;
		public int Length;
		public int Splits;
		public bool Playing;
		public bool Paused;
		public bool Repeat;
		public bool AutoMute;

		public double Progress;

		private DateTime _last = DateTime.MinValue;

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

			foreach (var stream in RingingStrings)
			{
				BassManager.Mute(stream);
			}
		}

		public int CountSkips(bool whole = false)
		{
			var max = whole ? Length - 1 : Math.Min(Length - 1, Index);
			var count = 0;
			for (int i = 1; i < max; i++)
			{
				if (IsSkip(i))
				{
					count++;
				}
			}
			return count;
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
				}/*
				else if (char.ToUpper(line[index]) == '|')
				{
					notes[i] = -3;
				}*/
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

		private bool IsSkip(int index)
		{
			for (int i = 0; i < Data.Length; i++)
			{
				var line = Data[i];

				if (index >= line.Length || line[index] != '|')
				{
					return false;
				}
			}

			return true;
		}

		private double GetDelta()
		{
			var now = DateTime.Now;

			double delta = 0;

			if (_last != DateTime.MinValue)
			{
				delta = (now - _last).TotalSeconds;
			}

			_last = now;

			return delta;
		}

		public void Update(bool skipStep = false)
		{
			var delta = GetDelta() * DashPerScond;

			if (skipStep)
			{
				Time = Math.Min(Length, Math.Max(1, Time));
			}

			if (Playing && !skipStep)
			{
				Time += delta;
			}

			var newIndex = (int)Math.Min(Length, Math.Floor(Time));
			var skips = 0;
			for (int i = Index; i <= newIndex; i++)
			{
				if (IsSkip(i) && !skipStep)
				{
					skips++;
				}
			}

			if (!skipStep)
			{
				newIndex += skips;

				for (int i = Index; i < newIndex; i++)
				{
					var notes = GetNotes(i);

					for (int strIndex = 0; strIndex < notes.Length; strIndex++)
					{
						var offset = notes[strIndex];
						if (offset > -1)
						{
							var note = Tuning[strIndex] + offset;
							var stream = Form1.Instance.NoteManager.Play(ref note, Instrument);

							OnPluck?.Invoke(this, new StringEventArgs(strIndex, false));

							if (strIndex < RingingStrings.Length)
							{
								if (AutoMute)
								{
									BassManager.Mute(RingingStrings[strIndex]);
								}

								RingingStrings[strIndex] = stream;
							}
						}
						else if (offset == -2)
						{
							if (strIndex < RingingStrings.Length)
							{
								BassManager.Mute(RingingStrings[strIndex]);
							}

							OnPluck?.Invoke(this, new StringEventArgs(strIndex, true));
						}
					}
				}

				Time += skips;
			}

			Index = newIndex;

			var over = Time - Length;

			if (over >= 0)
			{
				if (Repeat)
				{
					if (!skipStep && Playing)
					{
						Index = 0;
						Time = over;
					}
					else
					{
						Time = Length;
						Index = Length;
					}
				}
				else
				{
					Playing = false;
					Time = Length;
					Index = Length;
				}
			}

			Progress = Math.Max(0, Time - CountSkips() - 1) / (Length - Splits - 2); // -1 instead of -2 if we want stop to stop progress, -2 is stop to last dash
		}

		public static Tab Parse(string[] lines, bool merge, JSONInstrument instrument)
		{
			var started = false;
			var stringsSet = false;
			var strings = 0;
			var stringIndex = 0;

			var tuningOrig = instrument.Tuning.Select(n => Note.Parse(n)).ToArray();
			var tuning = new List<Note>();

			List<string> tab = new List<string>();

			for (int i = 0; i < lines.Length; i++)
			{
				var line = lines[i].Trim().Replace(" ", "");

				if (line.Contains("-") || line.Contains("|-") || line.Contains("-|") || line.Contains("-|-"))
				{
					started = true;

					var firstPipe = line.IndexOf('|');

					var note = tuningOrig[Math.Min(tuningOrig.Length - 1, stringIndex)];

					if (firstPipe > -1 && firstPipe <= 2)
					{
						if (firstPipe > 0)
						{
							note = Note.Parse(line.Substring(0, firstPipe), tuningOrig[Math.Min(tuningOrig.Length - 1, stringIndex)].Octave);
						}

						line = line.Substring(firstPipe, line.Length - firstPipe);
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
				var line = tab[i];

				if (merge)
				{
					line = $"|{line}|";

					line = line.Substring(1, line.Length - 2);
					line = line.Replace("|", "");

					line = $"|{line}|";
				}
				else
				{
					line = line.Replace("||", "|");
				}

				tab[i] = line;

				ringing[i] = -1;

				length = Math.Max(length, line.Length);
			}

			var t = new Tab()
			{
				Data = tab.ToArray(),
				Tuning = tuning.ToArray(),
				RingingStrings = ringing,
				Length = length,
				Instrument = instrument
			};

			t.Splits = t.CountSkips(true);

			return t;
		}
	}
}
