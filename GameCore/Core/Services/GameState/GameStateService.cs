using System.Threading.Tasks;
using GameCore.Core.Base.StateMachines;

namespace GameCore.Core.Services.GameState
{
    public class GameStateService : BaseStateMachine<IBaseGameState> //todo iService
    {
        protected override BaseStateContainer MakeContainer<TCurrentState>()
        {
            return new GameStateContainer(new TCurrentState());
        }

        protected override BaseStateContainer MakeContainer<TCurrentState, TStateArgs>(TStateArgs arguments)
        {
            return new GameStateContainer<TStateArgs>(new TCurrentState(), arguments);
        }
    }

    public class GameStateContainer : BaseStateContainer
    {
        public GameStateContainer(IState state)
        {
            State = state;
        }

        public override async Task ExitState()
        {
            await State.ExitState();
            ((IGameState)State).Unload();
        }

        public override async Task EnterState()
        {
            await ((IGameState)State).Load();
            await ((IGameState)State).EnterState();
        }
    }

    public class GameStateContainer<TStateArgs> : BaseStateContainer
        where TStateArgs : struct
    {
        public TStateArgs Arguments { get; private set; }

        public GameStateContainer(IState<TStateArgs> state, TStateArgs arguments)
        {
            State = state;
            Arguments = arguments;
        }

        public override async Task ExitState()
        {
            await State.ExitState();
            ((IGameState)State).Unload();
        }

        public override async Task EnterState()
        {
            await ((IGameState<TStateArgs>)State).Load(Arguments);
            await ((IGameState<TStateArgs>)State).EnterState(Arguments);
        }
    }
}
