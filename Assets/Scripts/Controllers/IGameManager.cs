using System;
using com.ktgame.core.manager;

namespace GamManager
{
    public interface IGameManager : IManager
    {
        GameManager.eStateGame State { get; }
        void ClearLevel();
        void SetState(GameManager.eStateGame state);
        event Action<GameManager.eStateGame> StateChangedAction;
        void LoadLevel(GameManager.eLevelMode mode);
        void RestartLevel();
    }

}
