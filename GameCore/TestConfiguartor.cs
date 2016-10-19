using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GameCore.Core.Application.Interfaces;

namespace GameCore
{
    public class TestConfiguartor : IGameInitializeConfigurator
    {
        public Type StartGameState { get { return typeof (TestGameState); } }
        public ReadOnlyCollection<Type> PredefinedServicesTypes {
            get { return new ReadOnlyCollection<Type>(new List<Type>
            {
                typeof(TestUIService)
            });}
        }
    }
}
