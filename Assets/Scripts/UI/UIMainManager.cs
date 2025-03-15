using System.Linq;
using com.ktgame.core.di;
using Enums;
using GamManager;
using UnityEngine;
using UnityEngine.UI;

public class UIMainManager : MonoBehaviour
{
    public IGameManager GameManager => m_gameManager;

    private IMenu[] m_menuList;

    [Inject] private IGameManager m_gameManager;

    private void Awake()
    {
        m_menuList = GetComponentsInChildren<IMenu>(true);
    }

    void Start()
    {
        for (int i = 0; i < m_menuList.Length; i++)
        {
            m_menuList[i].Setup(this);
        }
    }

    internal void ShowMainMenu()
    {
        m_gameManager.ClearLevel();
        m_gameManager.SetState(StateGame.MAIN_MENU);
    }

    void Update()
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

    internal void Setup(IGameManager gameManager)
    {
        m_gameManager = gameManager;

        m_gameManager.StateChangedAction += OnGameStateChange;
    }

    private void OnGameStateChange(StateGame state)
    {
        switch (state)
        {
            case StateGame.SETUP:
                break;
            case StateGame.MAIN_MENU:
                ShowMenu<UIPanelMain>();
                break;
            case StateGame.GAME_STARTED:
                ShowMenu<UIPanelGame>();
                break;
            case StateGame.PAUSE:
                ShowMenu<UIPanelPause>();
                break;
            case StateGame.GAME_OVER:
                ShowMenu<UIPanelGameOver>();
                break;
            case StateGame.RESTART:
                ShowMenu<UIPanelGameOver>();
                break;
        }
    }

    private void ShowMenu<T>() where T : IMenu
    {
        for (int i = 0; i < m_menuList.Length; i++)
        {
            IMenu menu = m_menuList[i];
            if (menu is T)
            {
                menu.Show();
            }
            else
            {
                menu.Hide();
            }
        }
    }

    internal Text GetLevelConditionView()
    {
        UIPanelGame game = m_menuList.Where(x => x is UIPanelGame).Cast<UIPanelGame>().FirstOrDefault();
        if (game)
        {
            return game.LevelConditionView;
        }

        return null;
    }

    internal void ShowPauseMenu()
    {
        m_gameManager.SetState(StateGame.PAUSE);
    }

    internal void LoadLevelMoves()
    {
        m_gameManager.LoadLevel(LevelMode.MOVES);
    }

    internal void LoadLevelTimer()
    {
        m_gameManager.LoadLevel(LevelMode.TIMER);
    }

    internal void ShowGameMenu()
    {
        m_gameManager.SetState(StateGame.GAME_STARTED);
    }
}