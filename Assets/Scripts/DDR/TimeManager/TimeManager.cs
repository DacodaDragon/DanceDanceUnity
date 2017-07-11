using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DDR.Timing;


namespace DDR
{
    // My brain hurts..

    /// <summary>
    /// TimeManager, manages timing data.
    /// </summary>
    public class TimeManager : MonoBehaviour
    {
        private string m_sFile; // For Information..
        private float m_beatZeroOffsetInSeconds;
        private List<BPMSegment> m_BPMSegments = new List<BPMSegment>();
        private List<StopSegment> m_StopSegments = new List<StopSegment>();

        // A small part that shouldn't be here, but its a work around
        // from source to DanceDanceUnity.. This should be replaced later
        // and not use magic numbers..
        #region NoteRows
        int BeatToNoteRow(float f) { return Mathf.RoundToInt(f * 48); }
        float NoteRowToBeat(int Row) { return Row / 48; }
        #endregion


        public TimeManager()
        {
            m_beatZeroOffsetInSeconds = 0;
        }

        private void SortBMPSegmentsArray()
        {
            // [TODO] Sort BMPS Segments
        }

        private void SortStopSegmentsArray()
        {
            // [TODO] Sort Stop Segments
        }

        public void GetActualBmp(out float MinBMP, out float MaxBMP)
        {
            MinBMP = float.MaxValue;
            MaxBMP = 0;

            for (int i = 0; i < m_BPMSegments.Count; i++)
            {
                BPMSegment seg = m_BPMSegments[i];
                MaxBMP = Mathf.Max(seg.m_BPS * 60, MaxBMP);
                MinBMP = Mathf.Min(seg.m_BPS * 60, MinBMP);
            }
        }

        public float GetBPMAtBeat(float beat)
        {
            int Index = BeatToNoteRow(beat);
            int i;
            for (i = 0; i < m_BPMSegments.Count - 1; i++)
                if (m_BPMSegments[i + 1].m_startIndex > Index)
                    break;
            return m_BPMSegments[i].GetBpm();
        }

        public int GetBPMSegmentIndexAtBeat(float beat)
        {
            int Index = BeatToNoteRow(beat);
            int i;
            for (i = 0; i < m_BPMSegments.Count - 1; i++)
                if (m_BPMSegments[i + 1].m_startIndex > Index)
                    break;
            return i;
        }

        public BPMSegment GetBMPSegmentAtBeat(float beat)
        {
            if (m_BPMSegments.Count == 0)
            {
                return new BPMSegment();
            }

            int i = GetBPMSegmentIndexAtBeat(beat);
            return m_BPMSegments[i];
        }

        public void SetBmpAtBeat(float beat, float BPM)
        {
            int NoteRow = BeatToNoteRow(beat);
            float BPS = BPM / 60;
            int i;

            for (i = 0; i < m_BPMSegments.Count; i++)
                if (m_BPMSegments[i].m_startIndex >= NoteRow)
                    break;

            if (i == m_BPMSegments.Count || m_BPMSegments[i].m_startIndex != NoteRow)
            {
                if (i == 0 || Mathf.Abs(m_BPMSegments[i-1].m_BPS - BPS) > 1e-5f)
                {
                    AddBPMSegment(new BPMSegment(NoteRow,BPM));
                }
            }
            else
            {
                if (i > 0 && Mathf.Abs(m_BPMSegments[i - 1].m_BPS - BPS) < 1e-5f)
                    m_BPMSegments.RemoveAt(i);
                else
                    m_BPMSegments[i].SetBpm(BPM);
            }
        }

        public void SetStopAtBeat(float beat, float seconds)
        {
            int NoteRow = BeatToNoteRow(beat);
            int i;

            for (i = 0; i < m_StopSegments.Count; i++)
                if (m_StopSegments[i].m_startRow == NoteRow)
                    break;

            if (i == m_StopSegments.Count)
            {
                if (seconds > 0)
                    AddStopSegment(new StopSegment(seconds, NoteRow));
            }
            else
            {
                if (seconds > 0)
                    m_StopSegments[i].SetSeconds(seconds);
                else
                    m_StopSegments.RemoveAt(i);
            }
        }

        public void MultiplyBPMInBeatRange(int IStartIndex, int IEndIndex, float Factor)
        {
            // TODO: Implement This.
            throw new System.NotImplementedException("Coming soon<3");
        }

        public void AddBPMSegment(BPMSegment segment)
        {
            m_BPMSegments.Add(segment);
            SortBMPSegmentsArray();
        }

        public void AddStopSegment(StopSegment segment)
        {
            m_StopSegments.Add(segment);
            SortStopSegmentsArray();
        }

        public void GetBeatAndBPSFromElapsedTime(float ElapsedTime, out float BeatOut, out float BPSOut, out bool FreezeOut)
        {
            // To comply with C#'s whining..
            BeatOut = -1;
            BPSOut = -1;
            FreezeOut = false;

            // This is supposed to be a settable amount from the menu
            // to account for screen response delay. It's not implemented in
            // DDU yet.
            // ElapsedTime += Globalseconds;

            ElapsedTime += m_beatZeroOffsetInSeconds;

            // For each Beat Segment
            for (int i = 0; i < m_BPMSegments.Count; i++)
            {
                int StartRowThisSegment = m_BPMSegments[i].m_startIndex;
                float StartBeatThisSegment = NoteRowToBeat(StartRowThisSegment);
                bool isFirstBMPSegment = (i == 0);
                bool isLastBMPSegment = (i == m_BPMSegments.Count - 1);

                // (1<<30) = 1073741824 = 1GB in bytes.. wonderfull!
                int StartRowNextSegment = isLastBMPSegment ? (1 << 30) : m_BPMSegments[i + 1].m_startIndex;
                float StartBeatNextSegment = NoteRowToBeat(StartRowNextSegment);
                float BPS = m_BPMSegments[i].m_BPS;

                // For each StopSegment/Freeze
                for (int j = 0; j < m_StopSegments.Count; j++)
                {
                    if (!isFirstBMPSegment && StartRowThisSegment >= m_StopSegments[j].m_startRow)
                        continue;
                    if (isLastBMPSegment && m_StopSegments[j].m_startRow > StartRowNextSegment)
                        continue;

                    int RowsBeatsSinceStartOfSegment = m_StopSegments[j].m_startRow - StartRowThisSegment;
                    float BeatsSinceStartOfSegment = NoteRowToBeat(RowsBeatsSinceStartOfSegment);
                    float FreezeStartSecond = BeatsSinceStartOfSegment / BPS;

                    if (FreezeStartSecond >= ElapsedTime)
                        break;
                    ElapsedTime -= m_StopSegments[j].m_StopSeconds;

                    if (FreezeStartSecond >= ElapsedTime)
                    {
                        BeatOut = NoteRowToBeat(m_StopSegments[j].m_startRow);
                        BPSOut = BPS;
                        FreezeOut = true;
                        return;
                    }
                }

                float BeatsInThisSegment = StartBeatNextSegment - StartBeatThisSegment;
                float SecondsInThisSegment = BeatsInThisSegment / BPS;
                if (isLastBMPSegment || ElapsedTime < SecondsInThisSegment)
                {
                    // This is the segment we are in.
                    BeatOut = StartBeatThisSegment + ElapsedTime * BPS;
                    BPSOut = BPS;
                    FreezeOut = false;
                    return;
                }

                // this BMPSegment is NOT the current segment.
                ElapsedTime -= SecondsInThisSegment;
            }
        }

        public float GetBeatFromElapsedTime(float ElapsedTime)
        {
            float beat;
            float fThrowAway;
            bool bThrowAway;
            GetBeatAndBPSFromElapsedTime(ElapsedTime, out beat, out fThrowAway, out bThrowAway);
            return beat;
        }

        public float GetElapsedTimeFromBeat(float beat)
        {
            float ElapsedTime = 0;
            ElapsedTime -= m_beatZeroOffsetInSeconds;

            int row = BeatToNoteRow(beat);
            for (int j = 0; j < m_StopSegments.Count; j++)
            {
                if (m_StopSegments[j].m_startRow >= row)
                {
                    break;
                }
                ElapsedTime += m_StopSegments[j].m_StopSeconds;
            }

            for (int i = 0; i < m_BPMSegments.Count; i++)
            {
                bool isLastSegment = i == m_BPMSegments.Count- 1;
                float bps = m_BPMSegments[i].m_BPS;

                if (isLastSegment)
                {
                    ElapsedTime += NoteRowToBeat(row) / bps;
                }
                else
                {
                    int StartIndexThisSegment = m_BPMSegments[i].m_startIndex;
                    int StartIndexNextSegment = m_BPMSegments[i+1].m_startIndex;
                    int RowsInThisSegment = Mathf.Min(StartIndexNextSegment - StartIndexThisSegment, row);
                    ElapsedTime += NoteRowToBeat(row) / bps;
                    row -= RowsInThisSegment;
                }
                if (row <= 0)
                    return ElapsedTime;
            }
            return ElapsedTime;
        }

        public bool HasBpmChanges()
        {
            return m_BPMSegments.Count > 1;
        }

        public bool HasStops()
        {
            return m_StopSegments.Count > 1;
        }

        static bool CompareBMPSegments(BPMSegment seg1, BPMSegment seg2)
        {
            // Source says < from Cpp returns int? might be <<.
            return seg1.m_startIndex < seg2.m_startIndex;
        }

        static bool CompareStopSegments(StopSegment seg1, StopSegment seg2)
        {
            // Source says < from Cpp returns int? might be <<.
            return seg1.m_startRow < seg2.m_startRow;
        }
    }
}

