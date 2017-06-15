using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDR.newloading
{
    class StepfileInterpreterSM
    {
        public static SongData GetSongDataFromString(string[] contents)
        {
            SongData songData = new SongData();
            songData.Empty();
            for (int i = 0; i < contents.Length; i++)
            {
                if (contents[i].StartsWith("#"))
                {
                    Interpret(ref songData, ref i, ref contents);
                }
                else continue;
            }
            return songData;
        }

        private static void Interpret(ref SongData songdata, ref int iterator, ref string[] contents)
        {
            if (contents[iterator].StartsWith("#"))
            {
                if (contents[iterator].StartsWith("#NOTES"))
                {
                    songdata.AddNoteData(GetNoteData(ref iterator, ref contents));
                }
                else songdata.AddParam(GetVariable(contents[iterator]));
            }
        }

        private static NoteData GetNoteData(ref int iterator, ref string[] contents)
        {
            NoteData noteData = new NoteData();
            iterator++;
            noteData.Mode = new StringBuilder(contents[iterator].Trim()).Remove(contents[iterator].Trim().Length-1,1).ToString();
            iterator++;
            noteData.Description = new StringBuilder(contents[iterator].Trim()).Remove(contents[iterator].Trim().Length-1,1).ToString();
            iterator++;
            noteData.Name = new StringBuilder(contents[iterator].Trim()).Remove(contents[iterator].Trim().Length-1,1).ToString();
            iterator++;
            noteData.Meter = new StringBuilder(contents[iterator].Trim()).Remove(contents[iterator].Trim().Length-1,1).ToString();
            iterator++;

            GetNoteSegments(ref iterator, ref contents, ref noteData);
            return noteData;
        }

        private static void GetNoteSegments(ref int iterator, ref string[] content, ref NoteData notedata)
        {
            Segment segment = new Segment();
            segment.rows = new List<Row>();
            List<Segment> segments = new List<Segment>();

            for (int i = iterator; i < content.Length; i++)
            {
                if (content[i].StartsWith(","))
                {
                    segments.Add(segment);
                    segment = new Segment();
                    segment.rows = new List<Row>();
                    continue;
                }
                else if (content[i].StartsWith(";"))
                {
                    segments.Add(segment);
                    break;
                }
                else
                {
                    Row row = new Row();
                    row.L = content[i][0] == '1';
                    row.D = content[i][1] == '1';
                    row.U = content[i][2] == '1';
                    row.R = content[i][3] == '1';
                    segment.rows.Add(row);
                }
            }

            SongdataToNotedata(segments.ToArray(),ref notedata);
        }

        private static void SongdataToNotedata(Segment[] segments, ref NoteData noteData)
        {
            List<float> L = new List<float>();
            List<float> D = new List<float>();
            List<float> U = new List<float>();
            List<float> R = new List<float>();

            for (int i = 0; i < segments.Length; i++)
            {
                for (int j = 0; j < segments[i].rows.Count; j++)
                {
                    Row row = segments[i].rows[j];
                    if (row.L) L.Add(((4f / segments[i].rows.Count) * j) + i * 4);
                    if (row.D) D.Add(((4f / segments[i].rows.Count) * j) + i * 4);
                    if (row.U) U.Add(((4f / segments[i].rows.Count) * j) + i * 4);
                    if (row.R) R.Add(((4f / segments[i].rows.Count) * j) + i * 4);
                }
            }

            noteData.L = L;
            noteData.D = D;
            noteData.U = U;
            noteData.R = R;
        }

        // ToDo: Unmagicfy
        private static KeyValuePair<string, string> GetVariable(string varString)
        {
            string[] splittedVar = varString.Split(':');
            string key = new StringBuilder(splittedVar[0]).Remove(0, 1).ToString();
            string value = new StringBuilder(splittedVar[1]).Remove(splittedVar[1].Length - 1, 1).ToString();
            return new KeyValuePair<string, string>(key, value);
        }
    }
}