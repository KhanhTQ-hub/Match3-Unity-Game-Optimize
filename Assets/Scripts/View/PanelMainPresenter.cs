using com.ktgame.core.di;
using com.ktgame.manager.ui;
using Cysharp.Threading.Tasks;
using Enums;
using GamManager;

namespace View
{
    public class PanelMainPresenter : ModalPresenter<PanelMainView>
    {
        [Inject] private readonly IGameManager _gameManager;
        [Inject] private readonly IMainViewManager _mainViewManager;
        public PanelMainPresenter(IUIManager uiManager, IViewContainer viewContainer, IViewConfig viewConfig) : base(uiManager, viewContainer, viewConfig)
        {
        }

        protected override void AddChildren()
        {
            
        }

        protected override UniTask ViewWillEnter(PanelMainView view)
        {
            view.SetOnMoves(OnMovesHandler);
            view.SetOnTimer(OnTimeHandler);
            return base.ViewWillEnter(view);
        }

        protected override UniTask ViewWillExit(PanelMainView view)
        {
            view.SetOnMoves(null);
            view.SetOnTimer(null);
            return base.ViewWillExit(view);
        }

        private void OnMovesHandler()
        {
            Hide(false);
            _gameManager.LoadLevel(LevelMode.MOVES);
             
            // if (mode == LevelMode.MOVES)
            // {
            //     m_levelCondition = this.gameObject.AddComponent<LevelMoves>();
            //     //       m_levelCondition.Setup(m_gameSettings.LevelMoves, _mainViewManager.GetLevelConditionView(), m_boardController);
            // }
            // else if (mode == LevelMode.TIMER)
            // {
            //     m_levelCondition = this.gameObject.AddComponent<LevelTime>();
            //     //   m_levelCondition.Setup(m_gameSettings.LevelMoves, _mainViewManager.GetLevelConditionView(), this);
            // }
        }

        protected override void ViewDidExit(PanelMainView view)
        {
            base.ViewDidExit(view);
        }

        private void OnTimeHandler()
        {
            Hide();
            _gameManager.LoadLevel(LevelMode.TIMER);
        }
    }
}
