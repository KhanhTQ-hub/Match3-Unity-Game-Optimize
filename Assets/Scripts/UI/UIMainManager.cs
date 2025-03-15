using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.ktgame.core.di;
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
        m_gameManager.SetState(GamManager.GameManager.eStateGame.MAIN_MENU);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_gameManager.State == GamManager.GameManager.eStateGame.GAME_STARTED)
            {
                m_gameManager.SetState(GamManager.GameManager.eStateGame.PAUSE);
            }
            else if (m_gameManager.State == GamManager.GameManager.eStateGame.PAUSE)
            {
                m_gameManager.SetState(GamManager.GameManager.eStateGame.GAME_STARTED);
            }
        }
    }

    internal void Setup(IGameManager gameManager)
    {
        m_gameManager = gameManager;
        
        m_gameManager.StateChangedAction += OnGameStateChange;
    }

    private void OnGameStateChange(GameManager.eStateGame state)
    {
        switch (state)
        {
            case GamManager.GameManager.eStateGame.SETUP:
                break;
            case GamManager.GameManager.eStateGame.MAIN_MENU:
                ShowMenu<UIPanelMain>();
                break;
            case GamManager.GameManager.eStateGame.GAME_STARTED:
                ShowMenu<UIPanelGame>();
                break;
            case GamManager.GameManager.eStateGame.PAUSE:
                ShowMenu<UIPanelPause>();
                break;
            case GamManager.GameManager.eStateGame.GAME_OVER:
                ShowMenu<UIPanelGameOver>();
                break;
            case GamManager.GameManager.eStateGame.RESTART:
                ShowMenu<UIPanelGameOver>();
                break;
        }
    }

    private void ShowMenu<T>() where T : IMenu
    {
        for (int i = 0; i < m_menuList.Length; i++)
        {
            IMenu menu = m_menuList[i];
            if(menu is T)
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
        m_gameManager.SetState(GamManager.GameManager.eStateGame.PAUSE);
    }

    internal void LoadLevelMoves()
    {
        m_gameManager.LoadLevel(GamManager.GameManager.eLevelMode.MOVES);
    }

    internal void LoadLevelTimer()
    {
        m_gameManager.LoadLevel(GamManager.GameManager.eLevelMode.TIMER);
    }

    internal void ShowGameMenu()
    {
        m_gameManager.SetState(GamManager.GameManager.eStateGame.GAME_STARTED);
    }
}
