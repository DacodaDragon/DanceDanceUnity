using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDR
{
    public class Playfield : MonoBehaviour
    {
        public Color BlitColor;
        public Color StaticColor;

        Color From;
        Color To;
        Color Current;

        float lerpValue = 0;

        public void Start()
        {
            GameObject.Find("_MusicPlayer").GetComponent<Song>().OnWhole += Blitz;
            From = To = Current = StaticColor;
            SetColor(StaticColor);
        }

        public void Blitz()
        {
            SetColor(BlitColor);
            LerpToColor(StaticColor);
        }

        public void Update()
        {
            lerpValue += Mathf.Min(1, Time.deltaTime * 5);
            Current = Color.Lerp(From, To, lerpValue);
            GetComponent<Renderer>().material.color = Current;
        }

        public void SetColor(Color color)
        {
            From = color;
            Current = color;
        }

        public void LerpToColor(Color color)
        {
            From = Current;
            To = color;
            lerpValue = 0;
        }
    }
}