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
            var state = new StateContainer(new TCurrentState());
            _statesContainersStack.Push(state);
            await EnterToState(state);
        }

        public async Task PushState<TCurrentState, TStateArgs>(TStateArgs arguments)
            where TCurrentState : TState, IState<TStateArgs>, new()
            where TStateArgs : struct
        {
            var state = new StateContainer<TStateArgs>(new TCurrentState(), arguments);
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
            var state = new StateContainer(new TCurrentState());
            await EnterToState(state);
        }

        public async Task SetState<TCurrentState, TStateArgs>(TStateArgs arguments)
            where TCurrentState : TState, IState<TStateArgs>, new()
            where TStateArgs : struct
        {
            ClearStateStack();
            var state = new StateContainer<TStateArgs>(new TCurrentState(),arguments);
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
                await OnAfterExitState((TState) _currentStateContainer.State);
            }
            _currentStateContainer = stateContainer;
            await OnBeforeEnterState((TState)_currentStateContainer.State);
            await _currentStateContainer.EnterState();
        }

        protected abstract Task OnBeforeEnterState(TState state);

        protected abstract Task OnAfterExitState(TState state);
    }
}
