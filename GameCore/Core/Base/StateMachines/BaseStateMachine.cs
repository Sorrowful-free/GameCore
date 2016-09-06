using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCore.Core.Extentions;

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
        private Stack<object[]> _stateParamsStack = new Stack<object[]>();

        public async Task PushState<TCurrentState>(params object[] paramsForEnter) where TCurrentState : TState
        {
            var typeOfState = typeof(TCurrentState);
            _statesStack.Push(typeOfState);
            _stateParamsStack.Push(paramsForEnter);
            await SetState<TCurrentState>(paramsForEnter);
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

        public async Task SetState<TCurrentState>(params object[] enterParams) where TCurrentState:TState
        {
            await SetState(typeof (TCurrentState), enterParams);
        }
        
        public async Task SetState(Type stateType, params object[] enterParams)
        {
            if (CurrentState != null)
            {
                CurrentState.ExitState();
                CurrentState.Unload(); // специально не дожидаюсь выгрузки
            }
            var state = await Task<TState>.Factory.StartNew( ()=> (TState) Activator.CreateInstance(stateType) );
            await state.Preload();
            state.EnterState(enterParams);
        }
    }
}
