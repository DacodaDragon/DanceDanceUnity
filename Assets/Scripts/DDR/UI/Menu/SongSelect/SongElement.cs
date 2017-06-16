using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DDR.newloading;
using DDR.Audio;

public class SongElement : MonoBehaviour {

    [SerializeField] Text Label;
    [SerializeField] Button Button;
    SongData songData;
    AudioSource songSource;

    bool WaitingForFileLoad = false;

    void Update()
    {
        if (WaitingForFileLoad && AudioLoader.IsLoaded)
        {
            GameObject.Find("_GameMaster").GetComponent<GameMaster>().GamePlaySong(
                AudioLoader.GetClip(songData.RequestParam("TITLE")), songData
                );
            AudioLoader.Dispose();
            WaitingForFileLoad = false;
        }
    }

    public void SetSongElement(SongData songData)
    {
        Label.text = songData.RequestParam("TITLE");
        Button.onClick.AddListener(new UnityAction(PlayMusic));
        this.songData = songData;
    }

    public void PlayMusic()
    {
        AudioLoader.LoadSong(songData.GetRootFolder() + "\\" + songData.RequestParam("MUSIC"));
        WaitingForFileLoad = true;
    }
}
