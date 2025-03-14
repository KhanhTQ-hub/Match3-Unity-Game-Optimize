using com.ktgame.manager.ui;
using Cysharp.Threading.Tasks;
using View;

namespace Scenes.GamePlay
{
    public class GamePresenter : UIManager
    {
        private PanelMainPresenter _panelMainPresenter;
        protected override UniTask OnInitialize()
        {
            _panelMainPresenter = GetPresenter<PanelMainPresenter>();
            _panelMainPresenter.Show();
            return UniTask.CompletedTask;
        }

        protected override void OnDispose()
        {
            
        }
    }
}
