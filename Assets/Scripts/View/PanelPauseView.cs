using System;
using System.Collections;
using System.Collections.Generic;
using com.ktgame.manager.ui;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class PanelPauseView : ModalView
    {
        [SerializeField] private Button btnClose;
        [SerializeField] private Button btnSound;

        private Action _onClickClose;
        private Action _onClickSound;

        protected override void Awake()
        {
            base.Awake();
            btnClose.onClick.AddListener(OnClickClose);
            btnSound.onClick.AddListener(OnClickSound);
        }

        public void SetOnClickClose(Action onClickClose)
        {
            _onClickClose = onClickClose;
        }
        
        public void SetOnClickSound(Action onClickSound)
        {
            _onClickSound = onClickSound;
        }
        
        private void OnClickSound()
        {
            _onClickSound?.Invoke();
        }
        
        private void OnClickClose()
        {
            _onClickClose?.Invoke();
        }
    }
}