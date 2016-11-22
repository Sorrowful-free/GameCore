using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GameCore.Core.Application.Interfaces.Services;
using GameCore.Core.Services.GameState;

namespace GameCore.Core.Application
{
    public class GameApplicationStartParameters
    {
        private Type _startGameStateType;
        private List<Type> _startServicesTypes = new List<Type>();

        public Type StartGameStateType { get { return _startGameStateType; } }
        public ReadOnlyCollection<Type> StartServicesTypes { get { return _startServicesTypes.AsReadOnly(); } }
        
        public void SetStartGameState<TGameState>() where TGameState : IBaseGameState
        {
            _startGameStateType = typeof (TGameState);
        }

        public void AddStartService<TService>() where TService : IService
        {
            _startServicesTypes.Add(typeof(TService));
        }
    }
}
