using System.Threading.Tasks;
using GameCore.Core.Base.StateMachines;

namespace GameCore.Core.Services.GameState
{
    public interface IBaseGameState : IBaseState
    {
        
        Task Unload();
    }

    public interface IGameState : IBaseGameState, IState
    {
        Task Load();
    }

    public interface IGameState<TStateArgs> : IBaseGameState, IState<TStateArgs> where TStateArgs : struct
    {
        Task Load(TStateArgs args);
    }
}
