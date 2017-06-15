using System;
using System.Collections.Generic;
using UnityEngine;

namespace DDR.newloading
{
    public struct Row
    {
        public bool L, D, U, R;
    }

    public struct Segment
    {
        public List<Row> rows;
    }

    [System.Serializable]
    public struct SongData
    {
        [SerializeField] Dictionary<string, string> Variables;
        [SerializeField] List<NoteData> noteData;
        [SerializeField] string RootFolder;

        public string RequestParam(string parameter)
        {
            string value = string.Empty;
            Variables.TryGetValue(parameter, out value);
            return value;
        }

        public void AddParam(KeyValuePair<string, string> param)
        {
            Variables.Add(param.Key, param.Value);
        }

        public bool ContainsParam(string parameter)
        {
            return Variables.ContainsKey(parameter);
        }

        public bool IsValidStepfile()
        {
            return (ContainsParam("TITLE") &&
                    ContainsParam("BMPS") &&
                    ContainsParam("MUSIC") &&
                    noteData.Count > 0);
        }

        public string[] GetDifficulties()
        {
            List<string> difficulties = new List<string>();
            for (int i = 0; i < noteData.Count; i++)
            {
                difficulties.Add(noteData[i].Name);
            }
            return difficulties.ToArray();
        }

        public NoteData getNoteData(string difficulty)
        {
            for (int i = 0; i < noteData.Count; i++)
            {
                if (noteData[i].Name == difficulty)
                    return noteData[i];
            }

            return new NoteData();
        }

        public void AddNoteData(NoteData noteData)
        {
            this.noteData.Add(noteData);
        }

        public void Empty()
        {
            Variables = new Dictionary<string, string>();
            noteData = new List<NoteData>();
        }

        public void SetRootFolder(string path)
        {
            if (System.IO.Directory.Exists(path))
            {
                RootFolder = path;
            }
            else throw new Exception("No Valid Rootfolder passed, SongData is Sad..");
        }

        public string GetRootFolder()
        {
            return RootFolder;
        }
    }

    public struct NoteData
    {
        public string Mode;
        public string Description;
        public string Name;
        public string Meter;
        public List<float> L, D, U, R;

        public int GetNoteCount { get { return L.Count + D.Count + U.Count + R.Count; } }
    }
}
