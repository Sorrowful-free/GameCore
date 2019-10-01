using System;
using System.Threading.Tasks;
using Core.Runtime.Services;
using States.Declaration.Services.States;
using UnityEngine;

namespace States.Declaration.Services
{
    public interface IStatesService : IService, 
        IServiceUpdate, 
        IServiceLateUpdate, 
        IServiceFixedUpdate,
        
        IServiceJobUpdate,
        IServiceJobLateUpdate,
        IServiceJobFixedUpdate
    {
        bool IsInTransition { get; }
        IState CurrentState { get; }
        Task SetState(Type newStateType, params object[] enterArgs);
        Task SetState<TState>(params object[] enterArgs) where TState : IState, new();

        Task EnqueueState(Type nextStateType, params object[] enterArgs);
        Task EnqueueState<TState>(params object[] enterArgs) where TState : IState, new();

        Type DequeueState();

        void ClearStatesQueue();
    }
}