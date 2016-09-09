using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameCore.Core.Base.StateMachines
{
    public class BaseStateMachine<TState> where TState:class ,IState, new()
    {
        public TState CurrentState { get; private set; }
        public Type CurrentStateType
        {
            get
            {
                if (_statesStack.Count > 0)
                    return _statesStack.Peek();
                return null;
            }
        }
        
        private Stack<Type> _statesStack = new Stack<Type>();
        private Stack<BaseEnterStateArguments> _stateParamsStack = new Stack<BaseEnterStateArguments>();

        public async Task PushState<TCurrentState>(BaseEnterStateArguments arguments) where TCurrentState : TState
        {
            var typeOfState = typeof(TCurrentState);
            _statesStack.Push(typeOfState);
            _stateParamsStack.Push(arguments);
            await SetState<TCurrentState>(arguments);
        }

        public async Task PopState()
        {
            var typeOfState = _statesStack.Pop();
            var stateParams = _stateParamsStack.Pop();
            await SetState(typeOfState, stateParams);
        }

        public void ClearStateStack()
        {
            _statesStack.Clear();
            _stateParamsStack.Clear();
        }

        public async Task SetState<TCurrentState>(BaseEnterStateArguments arguments) where TCurrentState:TState
        {
            await SetState(typeof (TCurrentState), arguments);
        }
        
        public async Task SetState(Type stateType, BaseEnterStateArguments arguments)
        {
            if (CurrentState != null)
            {
                CurrentState.ExitState();
            }
            var state = await Task<TState>.Factory.StartNew( ()=> (TState) Activator.CreateInstance(stateType) );
           // state.EnterState(enterParams);
        }
    }
}
