using System;
using System.Linq;
using GameCore.Core.Application.Interfaces;
using GameCore.Core.Base.Dependency;
using GameCore.Core.Logging;
using GameCore.Core.Services.GameObjectPool;
using GameCore.Core.Services.GameState;
using GameCore.Core.Services.Resources;
using GameCore.Core.UnityThreading;
using UnityEngine;

namespace GameCore.Core.Application
{
    public partial class GameApplication
    {
        
        public static bool IsPause { get; private set; }

        public static void Pause(bool isPause)
        {
            IsPause = isPause;
            Time.timeScale = isPause ? 0 : 1;
            PauseServices(isPause);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static async void StartApplication()
        {
            try
            {
                new GameObject("GameApplication").AddComponent<GameApplication>();
                var configurator = DependencyInjector.GetDependency<IGameInitializeConfigurator>();
                await GetService<ResourceService>();
                await GetService<GameObjectPoolService>();
                var gameStateService = await GetService<GameStateService>();
                foreach (var serviceType in configurator.PredefinedServicesTypes)
                {
                    await GetService(serviceType);
                }
                await gameStateService.SetState(configurator.StartGameState);
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        private static async void StopApplication()
        {
            foreach (var service in _services.Keys.ToArray())
            {
                await DestroyService(service);
            }
        }
    }
}
