using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDR.Timing
{
    public struct StopSegment
    {
        public float m_StopSeconds;
        public int m_startRow;

        public StopSegment(float stopSeconds, int row)
        {
            m_StopSeconds = stopSeconds;
            m_startRow = row;
        }

        public void SetSeconds(float Seconds)
        {
            m_StopSeconds = Seconds;
        }

        public override bool Equals(object obj)
        {
            return this == (StopSegment)obj;
        }

        public static bool operator !=(StopSegment x, StopSegment y)
        {
            return !(x.m_StopSeconds == y.m_StopSeconds && x.m_startRow == y.m_startRow);
        }

        public static bool operator ==(StopSegment x, StopSegment y)
        {
            return (x.m_StopSeconds == y.m_StopSeconds && x.m_startRow == y.m_startRow);
        }
    }
}