using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDR
{
    public class AxisRotate : MonoBehaviour
    {

        [SerializeField]
        Vector3 effect;
        void Start()
        {
            GameObject.Find("_MusicPlayer").GetComponent<Song>().OnWhole += Rotate;
        }
        void Rotate()
        {
            transform.Rotate(effect);
        }
    }
}

