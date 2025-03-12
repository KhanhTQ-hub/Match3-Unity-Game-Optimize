using com.ktgame.manager.ui;
using Cysharp.Threading.Tasks;

namespace Scenes.GamePlay
{
    public class GamePresenter : UIManager
    {
        protected override UniTask OnInitialize()
        {
            return UniTask.CompletedTask;
        }

        protected override void OnDispose()
        {
            
        }
    }
}
