using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDR
{
    public class AxisBounce : MonoBehaviour
    {

        Vector3 position;

        [SerializeField] Song song;
        [SerializeField] Vector3 strength;

        float zoomValue;
        // Use this for initialization
        void Start()
        {
            song = GameObject.Find("_MusicPlayer").GetComponent<Song>();
            position = transform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
            float[] spectrum = song.getSpectrum();
            if (spectrum != null)
                zoomValue = Mathf.Lerp(zoomValue, ((spectrum[5] + spectrum[10] + spectrum[15]) / 3), 0.4f);
            transform.localPosition = position + (strength * zoomValue);
        }
    }
}

