using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameCore.Core.Base.StateMachines
{
    public abstract class BaseStateMachine<TState> where TState : IBaseState
    {
        private BaseStateContainer _currentStateContainer;
        private Stack<BaseStateContainer> _statesContainersStack = new Stack<BaseStateContainer>();
        
        public async Task PushState<TCurrentState>() where TCurrentState : TState, IState, new()
        {
            var state = MakeContainer<TCurrentState>();
            _statesContainersStack.Push(state);
            await EnterToState(state);
        }

        public async Task PushState<TCurrentState, TStateArgs>(TStateArgs arguments)
            where TCurrentState : TState, IState<TStateArgs>, new()
            where TStateArgs : struct
        {
            var state = MakeContainer<TCurrentState, TStateArgs>(arguments);
            _statesContainersStack.Push(state);
            await EnterToState(state);
        }

        public async Task PopState()
        {
            await EnterToState(_statesContainersStack.Pop());
        }

        public async Task SetState<TCurrentState>() where TCurrentState : TState, IState, new()
        {
            ClearStateStack();
            var state = MakeContainer<TCurrentState>();
            await EnterToState(state);
        }

        public async Task SetState<TCurrentState, TStateArgs>(TStateArgs arguments)
            where TCurrentState : TState, IState<TStateArgs>, new()
            where TStateArgs : struct
        {
            ClearStateStack();
            var state = MakeContainer<TCurrentState,TStateArgs>(arguments);
            await EnterToState(state);
        }

        public void ClearStateStack()
        {
            _statesContainersStack.Clear();
        }

        private async Task EnterToState(BaseStateContainer stateContainer)
        {
            if (_currentStateContainer != null)
            {
                await _currentStateContainer.ExitState();
            }
            _currentStateContainer = stateContainer;
            await _currentStateContainer.EnterState();
        }

        protected virtual BaseStateContainer MakeContainer<TCurrentState>()
            where TCurrentState : TState, IState, new ()
        {
            return new StateContainer(new TCurrentState());
        }

        protected virtual BaseStateContainer MakeContainer<TCurrentState, TStateArgs>(TStateArgs arguments)
             where TCurrentState : TState, IState<TStateArgs>, new()
            where TStateArgs : struct
        {
            return new StateContainer<TStateArgs>(new TCurrentState(), arguments);
        }

    }
}
