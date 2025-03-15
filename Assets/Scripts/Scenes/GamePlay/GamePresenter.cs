using com.ktgame.manager.ui;
using Cysharp.Threading.Tasks;
using UnityEngine;
using View;

namespace Scenes.GamePlay
{
    public class GamePresenter : UIManager
    {
        [SerializeField] private Camera _camera;
        
        private PanelMainPresenter _panelMainPresenter;
        protected override UniTask OnInitialize()
        {
            _panelMainPresenter = GetPresenter<PanelMainPresenter>();
            GetPresenter<PanelGamePresenter>().Show();
            _panelMainPresenter.Show();
            return UniTask.CompletedTask;
        }

        protected override void OnDispose()
        {
            
        }
    }
}
