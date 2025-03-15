using DG.Tweening;
using System;
using System.Collections;
using com.ktgame.core.di;
using Controllers;
using Cysharp.Threading.Tasks;
using Manager.Assets;
using UnityEngine;

namespace GamManager
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        [Inject] private readonly IAssetManager m_assetManager;
        public event Action<eStateGame> StateChangedAction = delegate { };
        public BoardController BoardController => m_boardController;

        public enum eLevelMode
        {
            TIMER,
            MOVES
        }

        public enum eStateGame
        {
            SETUP,
            MAIN_MENU,
            GAME_STARTED,
            PAUSE,
            GAME_OVER,
            RESTART
        }

        private eStateGame m_state;

        public eStateGame State
        {
            get { return m_state; }
            private set
            {
                m_state = value;

                StateChangedAction(m_state);
            }
        }


        private GameSettings m_gameSettings;

        private SkinModeController m_skinModeController;

        private BoardController m_boardController;

        private UIMainManager m_uiMenu;

        private LevelCondition m_levelCondition;

        private eLevelMode m_levelMode;

        public int Priority => 0;
        public bool IsInitialized { get; private set; }

        public async UniTask Initialize()
        {
            State = eStateGame.SETUP;

            m_gameSettings = await m_assetManager.AddressableLoad<GameSettings>("gamesettings").Task;
            m_skinModeController = Resources.Load<SkinModeController>(Constants.SKIN_MODE_CONTROLLER_PATH);

            m_uiMenu = FindObjectOfType<UIMainManager>();
            m_uiMenu.Setup(this);
            
            State = eStateGame.MAIN_MENU;
            
            IsInitialized = true;
        }
        
        void Update()
        {
            if (m_boardController != null) m_boardController.Update();
        }


        public void SetState(eStateGame state)
        {
            State = state;

            if (State == eStateGame.PAUSE)
            {
                DOTween.PauseAll();
            }
            else
            {
                DOTween.PlayAll();
            }
        }

        public void LoadLevel(eLevelMode mode)
        {
            m_boardController = new GameObject("BoardController").AddComponent<BoardController>();
            m_boardController.StartGame(this, m_gameSettings, m_skinModeController);

            m_levelMode = mode;
            if (mode == eLevelMode.MOVES)
            {
                m_levelCondition = this.gameObject.AddComponent<LevelMoves>();
                m_levelCondition.Setup(m_gameSettings.LevelMoves, m_uiMenu.GetLevelConditionView(), m_boardController);
            }
            else if (mode == eLevelMode.TIMER)
            {
                m_levelCondition = this.gameObject.AddComponent<LevelTime>();
                m_levelCondition.Setup(m_gameSettings.LevelMoves, m_uiMenu.GetLevelConditionView(), this);
            }

            m_levelCondition.ConditionCompleteEvent += GameOver;

            State = eStateGame.GAME_STARTED;
        }

        public void RestartLevel()
        {
            if (State != eStateGame.GAME_STARTED) return;

            ClearLevel();
            LoadLevel(m_levelMode);
        }

        public void GameOver()
        {
            StartCoroutine(WaitBoardController());
        }

        public void ClearLevel()
        {
            if (m_boardController)
            {
                m_boardController.Clear();
                Destroy(m_boardController.gameObject);
                m_boardController = null;
            }
        }

        private IEnumerator WaitBoardController()
        {
            while (m_boardController.IsBusy)
            {
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(1f);

            State = eStateGame.GAME_OVER;

            if (m_levelCondition != null)
            {
                m_levelCondition.ConditionCompleteEvent -= GameOver;

                Destroy(m_levelCondition);
                m_levelCondition = null;
            }
        }

        public void Dispose()
        {
        }
    }
}