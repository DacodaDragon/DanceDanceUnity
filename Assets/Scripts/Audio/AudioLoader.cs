using NAudio.Wave;
using UnityEngine;
using System.Threading;
namespace DDR.Audio
{
    public class AudioLoader
    {
        private static bool isLoaded = false;
        public static bool IsLoaded
        {
            get { return isLoaded; }
        }

        // Data we need to create the audio clip
        // Unity doesn't allow component creation outside the main Thread.
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
            Clip.SetData(Data, 0);
            isLoaded = false;
            Dispose();
            return Clip;
        }

        public static void LoadSong(string file)
        {
            isLoaded = false;
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
            ReadWaveStream(new Mp3FileReader(File));
        }

        private static void LoadWav()
        {
            ReadWaveStream(new WaveFileReader(File));
        }

        private static void ReadWaveStream(WaveStream audioStream)
        {
            // Convert
            WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(audioStream);
            float[] samplesPCM = new float[audioStream.Length];
            audioStream.ToSampleProvider().Read(samplesPCM, 0, (int)pcmStream.Length);

            // Store
            Data = samplesPCM;
            length = (int)audioStream.Length;
            waveFormat = audioStream.WaveFormat;

            // Cleanup
            audioStream.Dispose();

            // We has loaded
            isLoaded = true;
        }
    }
}

