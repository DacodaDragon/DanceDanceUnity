using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DDR
{
    public delegate void ArrowHitEvent(float timeDifference);

    public class Row : MonoBehaviour
    {
        [SerializeField] private float m_deleteRange;
        [SerializeField] private float m_minJudgeRange;
        [SerializeField] private float m_visibleRange;
        [SerializeField] private List<Arrow> Arrows;
        [SerializeField] private Queue<Arrow> HiddenArrows = new Queue<Arrow>();
        [SerializeField] private GameObject ArrowPrefab;

        public event ArrowHitEvent OnArrowHit;

        private Song m_Song;

        // Use this for initialization
        public void Load(List<float> beatstamps, ColorScheme scheme)
        {
            for (int i = 0; i < beatstamps.Count; i++)
            {
                Arrow a = Instantiate(ArrowPrefab).GetComponent<Arrow>();
                a.beatStamp = beatstamps[i];
                a.transform.parent = transform;
                a.GetComponent<Renderer>().material = scheme.Paint(a.beatStamp);
                a.gameObject.SetActive(false);

                HiddenArrows.Enqueue(a);
            }
        }

        public void Update()
        {
            GetNextVisibleArrow();
            UpdateArrowPositions();
            CheckMisses();
        }

        private void UpdateArrowPositions()
        {
            for (int i = 0; i < Arrows.Count; i++)
            {
                Arrows[i].SetPosition(
                    m_Song.GetTimeInBeat(),
                    m_Song.speedMultiplier);
            }
        }

        private void CheckMisses()
        {
            if (Arrows.Count > 0)
                if (FromZeroOffset(Arrows[0]) < -(m_deleteRange))
                {
                    Arrows[0].gameObject.SetActive(false);
                    Arrows.RemoveAt(0);
                    if (OnArrowHit != null)
                        OnArrowHit(float.PositiveInfinity);
                }
        }

        private Arrow GetNextVisibleArrow()
        {
            if (HiddenArrows.Count == 0)
                return null;
            if (Mathf.Abs(FromZeroOffset(
                HiddenArrows.Peek()))
                > m_visibleRange)
                return null;

            Arrow arrow = HiddenArrows.Peek();
            Arrows.Add(HiddenArrows.Dequeue());
            arrow.gameObject.SetActive(true);
            return arrow;
        }

        public void Hit()
        {
            if (Arrows.Count <= 0)
                return;

            ClosestArrow closestArrow = GetClosestArrow();
            if (closestArrow.offset < m_minJudgeRange)
            {
                closestArrow.arrow.gameObject.SetActive(false);
                Arrows.RemoveAt(closestArrow.index);
                if (OnArrowHit != null)
                    OnArrowHit(closestArrow.offset);
            }
        }

        public void SetSongReference(Song song)
        {
            m_Song = song;
        }

        private ClosestArrow GetClosestArrow()
        {
            if (Arrows.Count == 1)
            {
                return new ClosestArrow(Arrows[0], 0, Mathf.Abs(FromZeroOffset(Arrows[0])));
            }
            float offset = float.PositiveInfinity;

            int i = 0;
            while (true)
            {
                if (Mathf.Abs(FromZeroOffset(Arrows[i])) < offset)
                {
                    offset = Mathf.Abs(FromZeroOffset(Arrows[i]));
                    i++;
                }
                else break;
            }
            i--;

            return new ClosestArrow(Arrows[i], i, offset);
        }

        private float FromZeroOffset(Arrow Arrow)
        {
            return Arrow.beatStamp - m_Song.GetTimeInBeat();
        }

        private struct ClosestArrow
        {
            public Arrow arrow;
            public int index;
            public float offset;
            public ClosestArrow(Arrow arrow, int index, float offset)
            {
                this.arrow = arrow;
                this.index = index;
                this.offset = offset;
            }
        }

    }
}