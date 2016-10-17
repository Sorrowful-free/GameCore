﻿using System;
using GameCore.Core.Application.Interfaces;
using GameCore.Core.Base.Dependency;
using GameCore.Core.Services.GameState;
using GameCore.Core.Services.Resources;
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
                await InitializeService<ResourceService>();
                var gameStateService = await InitializeService<GameStateService>();
                foreach (var serviceType in configurator.PredefinedServicesTypes)
                {
                    await InitializeService(serviceType);
                }

                await gameStateService.SetState(configurator.StartGameState);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);   
            }
        }
    }
}
