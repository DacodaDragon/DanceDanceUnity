using NAudio.Wave;
using UnityEngine;
using System.Threading;

public class AudioLoader
{
    public static bool IsLoaded = false;


    private static float[] Data;
    private static int length;
    private static WaveFormat waveFormat;
    private static string File;

    public static void Dispose()
    {
        Data = null;
        waveFormat = null;
    }

    public static AudioClip GetClip(string Name)
    {
        AudioClip Clip = AudioClip.Create(Name, length, waveFormat.Channels, waveFormat.SampleRate, false);
        Clip.SetData(Data,0);
        IsLoaded = false;
        Dispose();
        return Clip;
    }

    public static void LoadSong(string file)
    {
        IsLoaded = false;
        File = file;
        Thread thread = new Thread(new ThreadStart(Load));
        thread.Start();
    }

    private static void Load()
    {
        if (File.ToLower().EndsWith(".mp3"))
            LoadMp3();
        if (File.ToLower().EndsWith(".wav"))
            LoadWav();
        
    }

    private static void LoadMp3()
    {
        Mp3FileReader audioReader = new Mp3FileReader(File);
        WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(audioReader);
        StorePCM(pcmStream);
        audioReader.Dispose();
    }

    private static void LoadWav()
    {
        WaveFileReader audioReader = new WaveFileReader(File);
        WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(audioReader);
        StorePCM(pcmStream);
        audioReader.Dispose();
    }

    private static void StorePCM(WaveStream waveStream)
    {
        float[] audioData = new float[waveStream.Length];
        Debug.Log((waveStream.Length * 4) / 1024);
        waveStream.ToSampleProvider().Read(audioData, 0, (int)waveStream.Length);
        Data = audioData;
        length = (int)waveStream.Length;
        waveFormat = waveStream.WaveFormat;
        IsLoaded = true;
    }
}
