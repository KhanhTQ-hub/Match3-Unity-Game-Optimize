using System;
using com.ktgame.manager.ui;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class PanelGameView : PanelView
    {
        public Text LevelConditionView => _levelConditionView;
        
        [SerializeField] private Text _levelConditionView;
        [SerializeField] private Button btnPause;
        [SerializeField] private Button btnRestart;

        private Action _onClickPause;
        private Action _onClickRestart;

        protected override void Awake()
        {
            base.Awake();

            btnPause.onClick.AddListener(OnClickPause);
            btnRestart.onClick.AddListener(OnClickRestart);
        }

        public void SetOnClickPause(Action onClickPause)
        {
            _onClickPause = onClickPause;
        }

        public void SetOnClickRestart(Action onClickRestart)
        {
            _onClickRestart = onClickRestart;
        }

        private void OnClickPause()
        {
            _onClickPause?.Invoke();
        }

        private void OnClickRestart()
        {
            _onClickRestart?.Invoke();
        }
    }
}