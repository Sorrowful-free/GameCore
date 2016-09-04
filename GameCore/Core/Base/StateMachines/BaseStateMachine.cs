using System;
using System.Collections.Generic;
using GameCore.Core.Extentions;

namespace GameCore.Core.Base.StateMachines
{
    public class BaseStateMachine<TState> where TState:class ,IState,new()
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

        public void PushState<TCurrentState>(params object[] paramsForEnter) where TCurrentState : TState
        {
            var typeOfState = typeof(TCurrentState);
            _statesStack.Push(typeOfState);
            _stateParamsStack.Push(paramsForEnter);
            return SetState<TCurrentState>(paramsForEnter);
        }

        public void PopState()
        {
            var lastIndex = _statesStack.Count - 1;
            var typeOfState = _statesStack[lastIndex];
            _statesStack.RemoveAt(lastIndex);
            var stateParams = _stateParamsStack[lastIndex];
            _stateParamsStack.RemoveAt(lastIndex);
            return SetState(typeOfState, stateParams);
        }

        public void ClearStateStack()
        {
            _statesStack.Clear();
            _stateParamsStack.Clear();
        }

        public void SetState<TCurrentState>(params object[] enterParams) where TCurrentState:TState
        {
           return SetState(typeof (TCurrentState), enterParams);
        }
        
        public void SetState(Type stateType, params object[] enterParams)
        {
           return InstanceFactory.CreateInstance(stateType)
                .ContinueWith(createInstanceTask => (callback =>
                {
                   
                }) );
            
        }

        protected virtual void OnSetState(TState state, Action callback, params object[] enterParams)
        {
            var newState = state;
            if (CurrentState != null)
            {
                CurrentState.ExitState();
                InstanceFactory.DestroyInstance(CurrentState);
            }
            CurrentState = newState;
            CurrentState.EnterState(enterParams);
            callback.SafeInvoke();
        }
    }
}
