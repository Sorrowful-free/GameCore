using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCore.Core.Base.StateMachines;

namespace GameCore.Core.Services.GameState
{
    public interface IBaseGameState : IBaseState
    {
        Task Load();
        Task Unload();
    }

    public interface IGameState : IState
    {

    }

    public interface IGameState<TStateArgs> : IState<TStateArgs> where TStateArgs : struct
    {

    }
}
