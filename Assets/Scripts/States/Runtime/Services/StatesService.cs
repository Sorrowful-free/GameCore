using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using States.Declaration.Services;
using States.Declaration.Services.States;
using Unity.Jobs;

namespace States.Runtime.Services
{
    public class StatesService : IStatesService
    {
        private Queue<(Type stateType, object[] enterArgs)> _statesQueue =
            new Queue<(Type stateType, object[] enterArgs)>();

        public bool IsInTransition { get; private set; }
        public IState CurrentState
        {
            get { return _currentState; }
            private set
            {
                _currentState = value;
                _currentStateUpdate = _currentState as IStateUpdate;
                _currentStateLateUpdate = _currentState as IStateLateUpdate;
                _currentStateFixedUpdate = _currentState as IStateFixedUpdate;

                _currentStateJobUpdate = _currentState as IStateJobUpdate;
                _currentStateJobLateUpdate = _currentState as IStateJobLateUpdate;
                _currentStateJobFixedUpdate = _currentState as IStateJobFixedUpdate;
            }
        }

        private IState _currentState;
        private IStateUpdate _currentStateUpdate;
        private IStateLateUpdate _currentStateLateUpdate;
        private IStateFixedUpdate _currentStateFixedUpdate;
        private IStateJobUpdate _currentStateJobUpdate;
        private IStateJobLateUpdate _currentStateJobLateUpdate;
        private IStateJobFixedUpdate _currentStateJobFixedUpdate;

        public Task Initialize()
        {
            return Task.CompletedTask;
        }

        public Task DeInitialize()
        {
            return Task.CompletedTask;
        }

        public void Update(float deltaTime)
        {
            _currentStateUpdate?.Update(deltaTime);
        }

        public void LateUpdate(float deltaTime)
        {
            _currentStateLateUpdate?.LateUpdate(deltaTime);
        }

        public void FixedUpdate(float fixedDeltaTime)
        {
            _currentStateFixedUpdate?.FixedUpdate(fixedDeltaTime);
        }

        public JobHandle JobUpdate(float deltaTime, JobHandle handle)
        {
            if (_currentStateJobUpdate != null)
                return _currentStateJobUpdate.JobUpdate(deltaTime, handle);
            return handle;
        }

        public JobHandle JobLateUpdate(float deltaTime, JobHandle handle)
        {
            if (_currentStateJobLateUpdate != null)
                return _currentStateJobLateUpdate.JobLateUpdate(deltaTime, handle);
            return handle;
        }

        public JobHandle JobFixedUpdate(float fixedDeltaTime, JobHandle handle)
        {
            if (_currentStateJobFixedUpdate != null)
                return _currentStateJobFixedUpdate.JobFixedUpdate(fixedDeltaTime, handle);
            return handle;
        }


        public async Task SetState(Type newStateType, params object[] enterArgs)
        {
            IsInTransition = true;
            if(_currentState != null)
                await _currentState.ExitState();
            
            CurrentState = (IState) Activator.CreateInstance(newStateType);
            await CurrentState.EnterState(enterArgs);
            IsInTransition = false;

            if (_statesQueue.Count > 0)
            {
                var nextStateTouple = _statesQueue.Dequeue();
                await SetState(nextStateTouple.stateType, nextStateTouple.enterArgs);
            }
        }

        public async Task SetState<TState>(params object[] enterArgs) where TState : IState, new()
        {
            await SetState(typeof(TState), enterArgs);
        }

        public async Task EnqueueState(Type nextStateType, params object[] enterArgs)
        {
            if (IsInTransition && _statesQueue.Peek().stateType != nextStateType)
            {
                _statesQueue.Enqueue((nextStateType,enterArgs));
            }
            await SetState(nextStateType, enterArgs);
        }


        public async Task EnqueueState<TState>(params object[] enterArgs) where TState : IState, new()
        {
            await EnqueueState(typeof(TState), enterArgs);
        }

        public Type DequeueState()
        {
            if (_statesQueue.Count == 0)
                return CurrentState.GetType();
            return _statesQueue.Dequeue().stateType;
        }

        public void ClearStatesQueue()
        {
            _statesQueue.Clear();
        }

        public void Dispose()
        {
            ClearStatesQueue();
            CurrentState?.ExitState();
        }
    }
}