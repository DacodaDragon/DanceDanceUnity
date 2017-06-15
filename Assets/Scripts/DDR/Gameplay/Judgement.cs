using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DDR
{
    [System.Serializable]
    public class Judge
    {
        [SerializeField] List<JudgeParam> m_JudgementParams;

        public void Sort()
        {
            m_JudgementParams = (from e in m_JudgementParams
                                 orderby e.precision ascending
                                 select e).ToList();
        }

        public Judgement JudgeHit(float beatPrecision)
        {
            for (int i = 0; i < m_JudgementParams.Count; i++)
            {
                if (beatPrecision < m_JudgementParams[i].precision)
                {
                    return new Judgement(m_JudgementParams[i].name, beatPrecision, i, m_JudgementParams.Count);
                }
            }

            // Fuck
            return new Judgement("Miss", float.PositiveInfinity, m_JudgementParams.Count, m_JudgementParams.Count); ;
        }
    }

    [System.Serializable]
    public struct JudgeParam
    {
        public float precision;
        public string name;
    }

    public struct Judgement
    {
        public string name;
        public float precision;
        public int amountOfJudgements;
        public int judgePosition;
        // public string amountoffucksGiVeNaboutStans0CD = "None.. sorry";

        public Judgement(string name, float precision, int judgePosition, int judgementCount)
        {
            this.name = name;
            this.precision = precision;
            this.judgePosition = judgePosition;
            this.amountOfJudgements = judgementCount;
        }
    }
}

