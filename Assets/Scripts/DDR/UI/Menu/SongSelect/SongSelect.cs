using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DDR.newloading;

public class SongSelect : MonoBehaviour
{
    [SerializeField]
    GameObject ElementPrefab;

    public void Start()
    {
        GameMaster gm = GameObject.Find("_GameMaster").GetComponent<GameMaster>();
        SongData[] songData = gm.Songs;
        for (int i = 0; i < songData.Length; i++)
        {
            GameObject obj = Instantiate(ElementPrefab);
            obj.transform.parent = gameObject.transform;
            obj.GetComponent<SongElement>().SetSongElement(songData[i]);
        }
    }
}
