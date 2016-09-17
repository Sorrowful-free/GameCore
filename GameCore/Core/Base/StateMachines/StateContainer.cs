using System.Threading.Tasks;

namespace GameCore.Core.Base.StateMachines
{
    public abstract class BaseStateContainer : IState
    {
        public IBaseState State { get; protected set; }
        public abstract Task ExitState();

        public abstract Task EnterState();
    }

    public class StateContainer : BaseStateContainer
    {
        public StateContainer(IState state)
        {
            State = state;
        }

        public override async Task ExitState()
        {
            await State.ExitState();
        }

        public override async Task EnterState()
        {
            await ((IState) State).ExitState();
        }
    }

    public class StateContainer<TStateArgs> : BaseStateContainer
        where TStateArgs : struct
    {
        public TStateArgs Arguments { get; private set; }

        public StateContainer(IState<TStateArgs> state,TStateArgs arguments)
        {
            State = state;
            Arguments = arguments;
        }

        public override async Task ExitState()
        {
            await State.ExitState();
        }

        public override async Task EnterState()
        {
            await ((IState<TStateArgs>)State).EnterState(Arguments);
        }
    }
}
