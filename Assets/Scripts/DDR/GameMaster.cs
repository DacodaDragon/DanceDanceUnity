﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DDR.newloading;
using UnityEngine.SceneManagement;
using DDR;

public class GameMaster : MonoBehaviour
{
    [SerializeField]
    string m_songFolder;

    [SerializeField]
    SongData[] m_songs;
    public SongData[] Songs { get { return m_songs; } }

    [SerializeField]
    Song m_musicPlayer;

    void Awake()
    {
        if (FindObjectsOfType<GameMaster>().Length > 1)
        {
            Destroy(gameObject);
        }
        else DontDestroyOnLoad(this);

        m_songs = SongListPreloader.LoadAll(Application.dataPath + "/" + m_songFolder);
    }

    void Start()
    {
        m_musicPlayer = GameObject.Find("_MusicPlayer").GetComponent<Song>();
    }

    public SongData[] GetAllSongs()
    {
        return m_songs;
    }

    public void PlayMusic(AudioClip musicClip, SongData sData)
    {
        m_musicPlayer.PlayFile(musicClip, sData);
    }

    public void GamePlaySong(AudioClip musicClip, SongData sData)
    {
        m_musicPlayer.SetFile(musicClip,sData);
        SceneManager.LoadScene(1);
        SceneManager.sceneLoaded += new UnityAction<Scene,LoadSceneMode>(OnSceneLoad);
    }

    private void OnSceneLoad(Scene s, LoadSceneMode m)
    {
        SceneManager.sceneLoaded -= new UnityAction<Scene, LoadSceneMode>(OnSceneLoad);
        m_musicPlayer.Play();
    }
}
