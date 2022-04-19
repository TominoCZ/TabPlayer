using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TabPlayer
{
    public static class Extensions
    {
        public static bool TryParseInt(this char c, out int val)
        {
            if (c >= 48 && c <= 57)
            {
                val = c - 48;

                return true;
            }

            val = 0;

            return false;
        }

        public static bool IsNumber(this char c)
        {
            return c >= 48 && c <= 57;
        }

        public static bool IsNumber(this string s)
        {
            return int.TryParse(s, out _);
        }
    }

    public class Tab
    {
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

        private void GetNotes(ref int[] notes, int index)
        {
            for (int i = 0; i < Data.Length; i++)
            {
                var line = Data[i];

                notes[i] = -1;

                if (index >= line.Length)
                    continue;

                var charAtPos = line[index];

                if (char.ToLower(charAtPos) == 'x')
                {
                    notes[i] = -2;
                }
                else if (charAtPos.TryParseInt(out var numAtPos))
                {
                    if (index > 0 && line[index - 1].IsNumber())
                    {
                        continue;
                    }
                    if (index < line.Length - 1 && line[index + 1].TryParseInt(out var next) && int.TryParse($"{charAtPos}{next}", out var combined))
                    {
                        notes[i] = combined;

                        continue;
                    }

                    notes[i] = numAtPos;
                }
            }
        }

        private bool IsSkip(int index)
        {
            var lines = 0;
            var count = 0;

            for (int i = 0; i < Data.Length; i++)
            {
                var line = Data[i];

                /*
				if (index >= line.Length || line[index] != '|')
				{
					return false;
				}
				*/

                if (index >= line.Length)
                {
                    continue;
                }

                if (line[index] == '|')
                {
                    count++;
                }

                lines++;
            }

            return count >= lines / 2 && count > 0;
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

            var newIndex = Math.Min(Length, (int)Time);
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

                var notes = new int[Data.Length];

                for (int i = Index; i < newIndex; i++)
                {
                    GetNotes(ref notes, i);

                    for (int strIndex = 0; strIndex < notes.Length; strIndex++)
                    {
                        var offset = notes[strIndex];
                        if (offset > -1)
                        {
                            var note = Tuning[strIndex] + offset;
                            var stream = MainForm.Instance.NoteManager.Play(ref note, Instrument);

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

            Progress = Math.Min(1, Math.Max(0, Math.Max(0, Time - CountSkips() - 1) / (Length - Splits - 2))); // -1 instead of -2 if we want stop to stop progress, -2 is stop to last dash
        }

        private List<FileNote> GetAllNotes()
        {
            var list = new List<FileNote>();
            var ringing = new FileNote[Data.Length];

            long tick = 0;

            for (int i = 0; i < Length; i++)
            {
                if (IsSkip(i))
                    continue;

                var notes = new int[Data.Length];
                GetNotes(ref notes, i);

                for (int strIndex = 0; strIndex < notes.Length; strIndex++)
                {
                    var offset = notes[strIndex];
                    if (offset > -1)
                    {
                        var note = Tuning[strIndex] + offset;
                        var fileNote = new FileNote { Note = note, Tick = tick };

                        list.Add(fileNote);

                        if (strIndex < ringing.Length)
                        {
                            if (AutoMute && ringing[strIndex] is FileNote last)
                            {
                                last.Muted = true;
                                last.Length = tick - last.Tick;
                            }

                            ringing[strIndex] = fileNote;
                        }
                    }
                    else if (offset == -2)
                    {
                        if (strIndex < ringing.Length && ringing[strIndex] is FileNote note)
                        {
                            note.Muted = true;
                            note.Length = tick - note.Tick;

                            ringing[strIndex] = null;
                        }
                    }
                }

                tick++;
            }

            return list;
        }

        public byte[] GenerateAudioFile()
        {
            var toDispose = new List<IDisposable>();
            var sources = new List<ISampleProvider>();
            var notes = GetAllNotes();
            for (int i = 0; i < notes.Count; i++)
            {
                var note = notes[i];

                var data = MainForm.Instance.NoteManager.GetRaw(ref note.Note, Instrument);
                if (data == null)
                    continue;

                var silence = new SilenceProvider(data.WaveFormat);
                var stream = new RawSourceWaveStream(data.Data, 0, data.Data.Length, data.WaveFormat);

                var offset = TimeSpan.FromSeconds(note.Tick / DashPerScond);
                var sample = stream.ToSampleProvider();
                if (note.Muted)
                {
                    var length = TimeSpan.FromSeconds(note.Length / DashPerScond);
                    sample = sample.Take(length);
                }

                if (note.Tick > 0)
                    sample = silence.ToSampleProvider().Take(offset).FollowedBy(sample);

                sources.Add(sample);

                toDispose.Add(stream);
            }
            if (sources.Count == 0)
                return new byte[] { };

            var mixer = new MixingSampleProvider(sources);
            var wave = mixer.ToWaveProvider();

            using (var ms = new MemoryStream())
            {
                WaveFileWriter.WriteWavFileToStream(ms, wave);

                var data = ms.ToArray();

                foreach (var item in toDispose)
                {
                    item.Dispose();
                }

                return data;
            }
        }

        public static bool TryParse(string[] lines, bool merge, JSONInstrument instrument, out Tab result)
        {
            result = null;

            var started = false;
            var stringsSet = false;
            var strings = 0;
            var stringIndex = 0;

            var tuningOrig = instrument.Tuning.Select(n => Note.Parse(n)).ToArray();
            var tuning = new List<Note>();

            List<string> tab = new List<string>();

            foreach (var t in lines)
            {
                var line = t.Trim();

                var tabEnd = line.LastIndexOf("-|");
                if (tabEnd > 0)
                {
                    line = line.Substring(0, tabEnd + 2);
                }

                line = line.Replace(" ", "");

                if (line.Contains("-") || line.Contains("|"))
                {
                    started = true;

                    var firstPipe = line.IndexOf('|');
                    var note = tuningOrig[Math.Min(tuningOrig.Length - 1, stringIndex)];

                    if (firstPipe > -1)
                    {
                        var whole = line.Substring(0, firstPipe);

                        if (firstPipe > 0)
                        {
                            var letterEnd = firstPipe;

                            var firstDash = line.IndexOf('-');
                            if (firstDash > 0)
                                letterEnd = Math.Min(letterEnd, firstDash);

                            whole = line.Substring(0, letterEnd);

                            if (Note.IsNote(whole))
                            {
                                if (Note.GetOctave(whole, out _))
                                    note = Note.Parse(whole);
                                else
                                    note = Note.Parse(whole, tuningOrig[Math.Min(tuningOrig.Length - 1, stringIndex)].Octave);
                            }
                            else
                            {
                                note = tuningOrig[Math.Min(tuningOrig.Length - 1, stringIndex)];
                            }
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

                if (line.LastOrDefault() != '|')
                {
                    var lastPipe = line.LastIndexOf('|');

                    line = line.Substring(0, lastPipe + 1);
                }

                line = $"|{line}|";

                if (merge)
                {
                    line = line.Substring(1, line.Length - 2);
                    line = line.Replace("|", "");
                    line = $"|{line}|";
                }

                line = Regex.Replace(line, @"(\|)+", "|");

                var tabEnd = line.LastIndexOf('|');
                if (tabEnd > 0)
                {
                    line = line.Substring(0, tabEnd + 1);
                }

                tab[i] = line;
                ringing[i] = -1;

                length = Math.Max(length, line.Length);
            }

            if (length <= 2)
            {
                return false;
            }

            result = new Tab()
            {
                Data = tab.ToArray(),
                Tuning = tuning.ToArray(),
                RingingStrings = ringing,
                Length = length,
                Instrument = instrument
            };

            result.Splits = result.CountSkips(true);

            return true;
        }
    }

    class FileNote
    {
        public Note Note;
        public long Tick;
        public long Length;
        public bool Muted;
    }
}
