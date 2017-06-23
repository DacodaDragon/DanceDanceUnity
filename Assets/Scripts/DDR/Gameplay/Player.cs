using UnityEngine;
using DDR.newloading;

namespace DDR
{
    public delegate void KeyEvent();
    public delegate void JudgementChangeEvent(string judgement);
    public delegate void ScoreChangeEvent(float percentage);

    public class Player : MonoBehaviour
    {
        [SerializeField] private int Diff;
        private event KeyEvent m_KeyLeftEvent;
        private event KeyEvent m_KeyDownEvent;
        private event KeyEvent m_KeyUpEvent;
        private event KeyEvent m_KeyRightEvent;

        private event KeyEvent m_KeyReleaseLeftEvent;
        private event KeyEvent m_KeyReleaseDownEvent;
        private event KeyEvent m_KeyReleaseUpEvent;
        private event KeyEvent m_KeyReleaseRightEvent;

        public event JudgementChangeEvent OnJudgementChange;
        public event ScoreChangeEvent OnScoreChange;

        [SerializeField] private Judge judgementMeasurements;
        [SerializeField] private ColorScheme colorScheme;

        [Space(10f)]
        [SerializeField]
        private Row m_rowLeft;
        [SerializeField] private Row m_rowDown;
        [SerializeField] private Row m_rowUp;
        [SerializeField] private Row m_rowRight;

        [Space(10f)]
        [SerializeField]
        private KeyCode m_KeyLeft;
        [SerializeField] private KeyCode m_KeyDown;
        [SerializeField] private KeyCode m_KeyUp;
        [SerializeField] private KeyCode m_KeyRight;

        [SerializeField]
        Song m_currentSong;


        private float percentage;
        NoteData noteData;


        public void Start()
        {
            m_currentSong = GameObject.Find("_MusicPlayer").GetComponent<Song>();
            SongData songData = m_currentSong.songData;
            noteData = songData.getNoteData(songData.GetDifficulties()[0]);

            m_rowLeft.Load(noteData.L, colorScheme);
            m_rowDown.Load(noteData.D, colorScheme);
            m_rowUp.Load(noteData.U, colorScheme);
            m_rowRight.Load(noteData.R, colorScheme);

            HookRow(m_rowLeft, ref m_KeyLeftEvent, ref m_KeyReleaseLeftEvent);
            HookRow(m_rowDown, ref m_KeyDownEvent, ref m_KeyReleaseDownEvent);
            HookRow(m_rowUp, ref m_KeyUpEvent, ref m_KeyReleaseUpEvent);
            HookRow(m_rowRight, ref m_KeyRightEvent, ref m_KeyReleaseRightEvent);
        }

        public void Update()
        {
            CheckInput();
        }

        #region Input
        private void CheckInput()
        {
            CheckKey(m_KeyLeft, m_KeyLeftEvent, m_KeyReleaseLeftEvent);
            CheckKey(m_KeyDown, m_KeyDownEvent, m_KeyReleaseDownEvent);
            CheckKey(m_KeyUp, m_KeyUpEvent, m_KeyReleaseUpEvent);
            CheckKey(m_KeyRight, m_KeyRightEvent, m_KeyReleaseRightEvent);
        }

        private void CheckKey(KeyCode key, KeyEvent keyDownEvent, KeyEvent keyUpEvent)
        {
            if (Input.GetKeyDown(key))
            {
                if (keyDownEvent != null)
                    keyDownEvent.Invoke();
            }

            if (Input.GetKeyUp(key))
            {
                if (keyUpEvent != null)
                    keyUpEvent.Invoke();
            }
        }
        #endregion

        #region Hooking
        public void HookRow(Row Row, ref KeyEvent keyEvent, ref KeyEvent KeyUpEvent)
        {
            Row.OnArrowHit += RecieveArrowHit;
            Row.SetSongReference(m_currentSong);
            keyEvent += Row.Hit;

            Hitmarker hitMarker = Row.GetComponentInChildren<Hitmarker>();

            if (hitMarker)
            {
                keyEvent += hitMarker.OnTap;
                KeyUpEvent += hitMarker.OnRelease;
            }
        }
        #endregion

        #region Judgement
        public void RecieveArrowHit(float precision)
        {
            Judgement j = judgementMeasurements.JudgeHit(precision);
            if (OnJudgementChange != null)
                OnJudgementChange.Invoke(j.name);

            // Hope this works D:
            percentage += ((((float)j.amountOfJudgements - j.judgePosition) / j.amountOfJudgements) / noteData.GetNoteCount) * 100;

            if (OnScoreChange != null)
                OnScoreChange.Invoke(percentage);
        }
        #endregion
    }
}