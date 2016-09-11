using System.Threading.Tasks;

namespace GameCore.Core.Base.StateMachines
{
    public interface IBaseState
    {
        Task ExitState();
    }
    public interface IState : IBaseState
    {
        Task EnterState();
    }

    public interface IState<TStateArgs> : IBaseState where TStateArgs : struct
    {
        Task EnterState(TStateArgs arguments);
    }
}