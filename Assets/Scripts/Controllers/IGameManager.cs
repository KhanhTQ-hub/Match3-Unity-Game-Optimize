using System;
using com.ktgame.core.manager;
using Enums;

namespace GamManager
{
    public interface IGameManager : IManager
    {
        StateGame State { get; }
        void ClearLevel();
        void SetState(StateGame state);
        event Action<StateGame> StateChangedAction;
        void LoadLevel(LevelMode mode);
        void RestartLevel();
    }

}
