using System.Collections.Generic;
using System.IO;
using UnityEngine;
using DDR.newloading;

public class SongListPreloader
{
    public static SongData[] LoadAll(string RootFolder)
    {
        if (!Directory.Exists(RootFolder))
            return null;

        return CheckFolder(RootFolder);
    }

    private static SongData[] CheckFolder(string FolderPath)
    {
        Debug.Log("Checking:" + FolderPath);
        // Check Current Folder for SM files..
        List<SongData> StepData = new List<SongData>();
        StepData.AddRange(CheckForSm(FolderPath));

        string[] Folder = Directory.GetDirectories(FolderPath);

        for (int i = 0; i < Folder.Length; i++)
        {
           StepData.AddRange(CheckFolder(Folder[i]));
        }

        return StepData.ToArray();
    }

    private static SongData[] CheckForSm(string FolderPath)
    {
        List<SongData> StepData = new List<SongData>();
        string[] files = Directory.GetFiles(FolderPath);

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].EndsWith(".sm"))
            {
                SongData sData = DDRSongLoader.Load(files[i]);
                sData.SetRootFolder(FolderPath);
                StepData.Add(sData);
            }
        }

        return StepData.ToArray();
    }
}
