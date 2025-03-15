using DG.Tweening;
using System;
using System.Collections;
using com.ktgame.core.di;
using Controllers;
using Cysharp.Threading.Tasks;
using Enums;
using Manager.Assets;
using UnityEngine;

namespace GamManager
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        [Inject] private readonly IAssetManager m_assetManager;
        [Inject] private readonly IMainViewManager _mainViewManager;
        [Inject] private readonly IInjector _injector;

        [SerializeField] private Camera m_cam;
        public event Action<StateGame> StateChangedAction = delegate { };
        public BoardController BoardController => m_boardController;

        private StateGame m_state;

        public StateGame State
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

        private LevelCondition m_levelCondition;

        private LevelMode m_levelMode;


        public int Priority => 0;
        public bool IsInitialized { get; private set; }

        public async UniTask Initialize()
        {
            State = StateGame.SETUP;

            m_gameSettings = await m_assetManager.AddressableLoad<GameSettings>("gamesettings").Task;
            m_skinModeController = await m_assetManager.AddressableLoad<SkinModeController>("skinmodecontroller").Task;

            State = StateGame.MAIN_MENU;

            IsInitialized = true;
        }

        void Update()
        {
            if (m_boardController != null && State == StateGame.GAME_STARTED)
            {
                m_boardController.Tick();
            }
        }


        public void SetState(StateGame state)
        {
            State = state;

            if (State == StateGame.PAUSE)
            {
                DOTween.PauseAll();
            }
            else
            {
                DOTween.PlayAll();
            }
        }

        public async void LoadLevel(LevelMode mode)
        {
            var boardController = await m_assetManager.AddressableLoad<GameObject>("Board/BoardControler").Task;
            _injector.Resolve(boardController);
            m_boardController = Instantiate(boardController).GetComponent<BoardController>();
            m_boardController.transform.position = Vector3.zero;
            m_boardController.transform.SetParent(this.transform);

            m_boardController.StartGame(this, m_gameSettings, m_skinModeController, m_cam);

            m_levelMode = mode;

            //m_levelCondition.ConditionCompleteEvent += GameOver;

            State = StateGame.GAME_STARTED;
        }

        public void RestartLevel()
        {
            if (State != StateGame.GAME_STARTED) return;

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

            State = StateGame.GAME_OVER;

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