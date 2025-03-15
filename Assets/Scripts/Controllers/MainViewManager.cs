using com.ktgame.core.di;
using Cysharp.Threading.Tasks;
using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace GamManager
{
    public class MainViewManager : MonoBehaviour, IMainViewManager
    {
        public int Priority => 0;
        public bool IsInitialized { get; private set; }
        
        [Inject] private IGameManager m_gameManager;
        
        public UniTask Initialize()
        {
            IsInitialized = true;
            return UniTask.CompletedTask;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (m_gameManager.State == StateGame.GAME_STARTED)
                {
                    m_gameManager.SetState(StateGame.PAUSE);
                }
                else if (m_gameManager.State == StateGame.PAUSE)
                {
                    m_gameManager.SetState(StateGame.GAME_STARTED);
                }
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}