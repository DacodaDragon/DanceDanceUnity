using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDR.Timing
{
    public struct BPMSegment
    {
        public int m_startIndex;
        public float m_BPS;

        public BPMSegment(int startIndex,float BPM)
        {
            m_startIndex = startIndex;
            m_BPS = BPM / 60;
        }

        public void SetBpm(float BMP)
        {
            m_BPS = BMP / 60;
        }

        public float GetBpm()
        {
            return m_BPS * 60;
        }

        public override bool Equals(object obj)
        {
            return this == (BPMSegment)obj;
        }

        public static bool operator !=(BPMSegment x, BPMSegment y)
        {
            return !(x.m_BPS == y.m_BPS && x.m_startIndex == y.m_startIndex);
        }

        public static bool operator ==(BPMSegment x, BPMSegment y)
        {
            return (x.m_BPS == y.m_BPS && x.m_startIndex == y.m_startIndex);
        }
    }
}