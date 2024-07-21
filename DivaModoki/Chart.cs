using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivaModoki
{
    /// <summary>
    /// Note(1拍)
    /// </summary>
    /// <param name="a"></param>
    internal class Note(int a)
    {
        int note => a;

        public int Data
        {
            get { return note; }
        }
    }

    /// <summary>
    /// 小節
    /// </summary>
    internal class Measure
    {
        double bpm;
        List<Note> notes;

        public double Bpm
        {
            get { return bpm; }
            set { bpm = value; }
        }

        public Measure(double bpm)
        {
            this.bpm = bpm;
            notes = new List<Note>();
        }

        public void AddNote(Note note)
        {
            notes.Add(note);
        }

        public void AddNotes(Note[] notes)
        {
            foreach (Note note in notes)
            {
                AddNote(note);
            }
        }

        public void WriteNotes(Note[] notes)
        {
            this.notes = new List<Note>(notes);
        }

        public Note[] GetNotes()
        {
            return notes.ToArray();
        }

        public void ClearNotes()
        {
            notes = new List<Note>();
        }

        public double GetLength()
        {
            return 60.0 / bpm * 4.0;
        }
    }

    /// <summary>
    /// 譜面
    /// </summary>
    internal class Chart()
    {
        List<Measure> measures = new List<Measure>();
        Dictionary<string, string> metadata = new Dictionary<string, string>();

        public void AddMeasure(Measure measure)
        {
            measures.Add(measure);
        }

        public void AddMeasures(Measure[] measures)
        {
            foreach (Measure measure in measures)
            {
                this.measures.Add(measure);
            }
        }

        public void ClearMeasures()
        {
            measures = new List<Measure>();
        }

        public void PeekNotes(int time, int range, out SortedDictionary<int, Note> notes)
        {
            notes = new SortedDictionary<int, Note>();

            double gtime = 0;

            for(int i = 0; i < measures.Count; i++)
            {
                double mlength = measures[i].GetLength();

                if (gtime >= time && gtime - time < range)
                {
                    double delta = gtime - time;

                    Note[] mnotes = measures[i].GetNotes();

                    for (int j = 0; j < mnotes.Length; j++)
                    {
                        double ltime = gtime + (mlength / mnotes.Length) * j;

                        if (ltime - time < range && mnotes[i].Data != 0)
                        {
                            int fdelta = (int)(ltime - time);
                            notes[fdelta] = mnotes[i];
                        }
                    }
                }

                gtime += mlength;
            }
        }

        public void SetMetadata(string key, string value)
        {
            metadata[key] = value;
        }

        public bool GetMetadata(string key, out string value, string defaultvalue = "Unknown")
        {
            if (metadata.ContainsKey(key))
            {
                value = metadata[key];
                return true;
            }
            else
            {
                value = defaultvalue;
                return false;
            }
        }
    }
}
