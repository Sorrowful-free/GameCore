using System;
using System.Collections.ObjectModel;

namespace GameCore.Core.Application.Interfaces
{
    public interface IGameInitializeConfigurator
    {
        Type StartGameState { get; }
        ReadOnlyCollection<Type> PredefinedServicesTypes { get; }
    }
}
