using System;
using com.ktgame.manager.ui;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class PanelMainView : ModalView
    {
        [SerializeField] private Button btnTimer;
        [SerializeField] private Button btnMoves;

        private Action _onTimer;
        private Action _onMoves;
        
        protected override void Awake()
        {
            base.Awake();
            btnMoves.onClick.AddListener(OnClickMoves);
            btnTimer.onClick.AddListener(OnClickTimer);
        }

        public void SetOnTimer(Action onTimer)
        {
            _onTimer = onTimer;
        }
        
        public void SetOnMoves(Action onMoves)
        {
            _onMoves = onMoves;
        }
        
        private void OnClickMoves()
        {
            _onTimer?.Invoke();
        }

        private void OnClickTimer()
        {
            _onMoves?.Invoke();
        }
    }
}