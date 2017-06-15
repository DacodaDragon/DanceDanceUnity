using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using System.Text;

namespace DDR.newloading
{
    public class DDRSongLoader
    {
        private static NoteData alreadyLoaded;
        private static bool isLoaded;

        static DDRSongLoader()
        {
            isLoaded = false;
        }

        public static SongData Load(string path)
        {
            return StepfileInterpreterSM.GetSongDataFromString(
                File.ReadAllLines(path)
                ); ;
        }
    }
}


