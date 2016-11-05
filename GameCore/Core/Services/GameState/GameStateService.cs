using System;
using System.Threading.Tasks;
using GameCore.Core.Application.Interfaces.Services;
using GameCore.Core.Base.StateMachines;
using GameCore.Core.Logging;

namespace GameCore.Core.Services.GameState
{
    public class GameStateService : BaseStateMachine<IBaseGameState>, IService
    {
        public async Task Initialize()
        {
            Log.Info("GameStateService initialize");
        }

        public async Task Deinitialize()
        {
            Log.Info("GameStateService deinitialize");
        }

        protected override BaseStateContainer MakeContainer<TCurrentState>()
        {
            return new GameStateContainer(new TCurrentState());
        }

        protected override BaseStateContainer MakeContainer<TCurrentState, TStateArgs>(TStateArgs arguments)
        {
            return new GameStateContainer<TStateArgs>(new TCurrentState(), arguments);
        }

        protected override BaseStateContainer MakeContainer(Type stateType)
        {
            var state = (IGameState)Activator.CreateInstance(stateType);
            return new GameStateContainer(state);
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
