using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDR
{
    public class Hitmarker : MonoBehaviour
    {

        public Color TapColor;
        public Color HoldColor;
        public Color StaticColor;
        public Color BlitzColor;

        Color From;
        Color To;
        Color Current;

        float lerpValue = 0;

        bool isPressed;

        public void Start()
        {
            From = To = Current = GetComponent<Renderer>().material.color = StaticColor;
            GameObject.Find("_MusicPlayer").GetComponent<Song>().OnWhole += Blitz;
        }

        public void Blitz()
        {
            if (!isPressed)
            {
                // Bork bork I am a shork.. what?
                SetColor(BlitzColor);
                LerpToColor(StaticColor);
            }
            else
            {
                OnTap();
            }
        }

        public void Update()
        {
            lerpValue += Mathf.Min(1, Time.deltaTime * 6);
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

        public void OnTap()
        {
            isPressed = true;
            SetColor(TapColor);
            LerpToColor(HoldColor);
        }

        public void OnRelease()
        {
            isPressed = false;
            SetColor(Current);
            LerpToColor(StaticColor);
        }
    }
}
