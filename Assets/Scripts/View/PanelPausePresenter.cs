using com.ktgame.core.di;
using com.ktgame.manager.ui;
using com.ktgame.services.audio;
using Cysharp.Threading.Tasks;
using Enums;
using GamManager;

namespace View
{
    public class PanelPausePresenter : ModalPresenter<PanelPauseView>
    {
        [Inject] private readonly IGameManager _gameManager;
        [Inject] private readonly IAudioManager _audioManager;
        
        private bool _isSoundOn = true;
        public PanelPausePresenter(IUIManager uiManager, IViewContainer viewContainer, IViewConfig viewConfig) : base(
            uiManager, viewContainer, viewConfig)
        {
        }

        protected override void AddChildren()
        {
        }

        protected override UniTask ViewWillEnter(PanelPauseView view)
        {
            view.SetOnClickClose(OnClickCloseHandler);
            view.SetOnClickSound(OnClickSoundHandler);
            
            return base.ViewWillEnter(view);
        }

        private void OnClickSoundHandler()
        {
            
        }

        protected override UniTask ViewWillExit(PanelPauseView view)
        {
            view.SetOnClickClose(null);
            view.SetOnClickSound(null);
            return base.ViewWillExit(view);
        }

        private void OnClickCloseHandler()
        {
            _gameManager.SetState(StateGame.GAME_STARTED);
            Hide();
        }
    }
}