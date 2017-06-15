using System.Collections;
using System.Collections.Generic;
using DDR.newloading;
using UnityEngine;

public delegate void OnBeatDelegate();

public class DDRSong : MonoBehaviour
{
    private AudioSource m_AudioSource;
    [SerializeField] private float m_time;
    [SerializeField] private float m_bmp;
    [SerializeField] private float m_preDelay;
    [SerializeField] private float m_ScreenDelayTime;
    [SerializeField] private float m_SpeedMultiplier;
    [SerializeField] private string NoteFile;

    public OnBeatDelegate OnWhole;

    public float NextWhole;

    private float[] spectrumData;
    public float speedMultiplier { get { return m_SpeedMultiplier; } }

    SongData m_songData;
    public SongData songData { get { return m_songData; } }

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void Update()
    {
        m_time = m_AudioSource.time;
        spectrumData = m_AudioSource.GetSpectrumData(1024,0,FFTWindow.Blackman);
        if (GetTimeInBeat() > NextWhole)
        {
            NextWhole += 1;
            if (OnWhole != null)
                OnWhole.Invoke();
        }
    }

    public float GetTime()
    {
        return m_time - m_preDelay;
    }

    public float[] getSpectrum()
    {
        return spectrumData;
    }

    public float GetTimeInBeat()
    {
        return BMP.TimeToBeat(m_time - m_preDelay, m_bmp);
    }

    public float GetBmp()
    {
        return m_bmp;
    }

    public void PlayFile(AudioClip clip, SongData songData)
    {
        // fock yeah
        SetFile(clip, songData);
        m_AudioSource.Play();
    }

    public void SetFile(AudioClip clip, SongData songData)
    {
        this.m_songData = songData;
        m_bmp = float.Parse(songData.RequestParam("BPMS").Split('=')[1]);
        m_preDelay = -(float.Parse(songData.RequestParam("OFFSET")) - m_ScreenDelayTime);
        m_AudioSource.clip = clip;
    }

    public void Play()
    {
        m_AudioSource.Play();
    }
}