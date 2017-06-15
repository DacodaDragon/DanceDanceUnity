using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DDR
{
    public class JudgementReciever : MonoBehaviour
    {
        private Text m_scoreLabel;
        [SerializeField] private Player m_player;

        void Start()
        {
            m_scoreLabel = GetComponent<Text>();
            m_player.OnJudgementChange += OnScoreChange;
        }

        private void OnScoreChange(string judgement)
        {
            m_scoreLabel.text = judgement;
        }
    }
}

