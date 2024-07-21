using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivaModoki
{
    internal class ChartLoader
    {
        StreamReader reader;

        public ChartLoader(StreamReader reader)
        {
            this.reader = reader;
        }

        public ChartLoader(string path)
        {
            this.reader = new StreamReader(path, Encoding.UTF8);
        }

        public bool Load(out Chart chart)
        {
            chart = new Chart();

            Chart pchart = new Chart();

            if(reader == null) return false;

            double pbpm = 120.0;

            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();

                if (line == null) return false;

                if (line.Length == 0) continue;

                if (line.StartsWith('#'))
                {
                    string[] tok = line.Substring(1).Split(new char[] { ':' }, 2);

                    if (tok.Length == 2)
                    {
                        tok[0] = tok[0].Trim().ToUpper();
                        tok[1] = tok[1].Trim();

                        if (tok[0] == "BPM")
                        {
                            bool res = double.TryParse(tok[1], out double tmp);
                            if (res) pbpm = tmp;
                        }
                        else if (
                            tok[0] == "TITLE" ||
                            tok[0] == "SUBTITLE" ||
                            tok[0] == "DIFFNAME" ||
                            tok[0] == "ARTIST" ||
                            tok[0] == "GENRE" ||
                            tok[0] == "CHARTER" ||
                            tok[0] == "DIFFICULTY" ||
                            tok[0] == "LEVEL")
                        {
                            pchart.SetMetadata(tok[0], tok[1]);
                        }
                        else if (tok[0].Length > 5 && tok[0].Substring(0, 5) == "META_")
                        {
                            pchart.SetMetadata(tok[0], tok[1]);
                        }
                    }
                }
                else
                {
                    string snotes = line.Trim()
                        .Replace(" ", string.Empty)
                        .Replace("\t", string.Empty)
                        .Replace("\r", string.Empty)
                        .Replace("\n", string.Empty);
                    
                    List<Note> notes = new List<Note>();

                    foreach (char ch in snotes)
                    {
                        notes.Add(new Note(ch));
                    }

                    Measure m = new Measure(pbpm);
                    m.AddNotes(notes.ToArray());

                    pchart.AddMeasure(m);
                }
            }

            chart = pchart;

            return true;
        }
    }
}
