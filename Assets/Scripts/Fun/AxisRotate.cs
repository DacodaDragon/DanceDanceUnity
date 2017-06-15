using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisRotate : MonoBehaviour {

    [SerializeField]
    Vector3 effect;
	void Start () {
        GameObject.Find("_MusicPlayer").GetComponent<DDRSong>().OnWhole += Rotate;
	}
    void Rotate()
    {
        transform.Rotate(effect);
    }
}
