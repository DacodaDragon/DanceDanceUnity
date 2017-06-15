using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreReciever : MonoBehaviour {

    [SerializeField]
    DDRPlayer player;

    [SerializeField]
    Text text;

    void Start()
    {
        player.OnScoreChange += OnScoreChanged;
        text = GetComponent<Text>();
    }

    private void OnScoreChanged(float percentage)
    {
        text.text = percentage.ToString();
    }
}
