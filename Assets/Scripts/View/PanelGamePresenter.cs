using com.ktgame.core.di;
using com.ktgame.manager.ui;
using Cysharp.Threading.Tasks;
using Enums;
using GamManager;

namespace View
{
    public class PanelGamePresenter : PanelPresenter<PanelGameView>
    {
        [Inject] private readonly IGameManager _gameManager;
        
        public PanelGamePresenter(IUIManager uiManager, IViewContainer viewContainer, IViewConfig viewConfig) : base(uiManager, viewContainer, viewConfig)
        {
        }

        protected override void AddChildren()
        {
            
        }

        protected override UniTask ViewWillEnter(PanelGameView view)
        {
            view.SetOnClickPause(OnClickPauseHandler);
            return base.ViewWillEnter(view);
        }

        private void OnClickPauseHandler()
        {
            _gameManager.SetState(StateGame.PAUSE);
        }
        
    }

}
