using System;
using System.Linq;
using System.Threading.Tasks;
using GameCore.Core.Logging;
using GameCore.Core.Services.GameObjectPool;
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

        public static async Task StartApplication()
        {
            try
            {
                new GameObject("GameApplication").AddComponent<GameApplication>();
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
                await DeinitializeService(service);
            }
        }
    }
}
