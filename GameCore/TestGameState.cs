using System.Threading.Tasks;
using GameCore.Core;
using GameCore.Core.Application;
using GameCore.Core.Services.GameState;
using UnityEngine;

namespace GameCore
{
    public class TestGameState : IGameState
    {
        public async Task ExitState()
        {
            
        }

        public async Task EnterState()
        {
            var service = await GameApplication.InitializeService<ITestService>();
            service.Hello();
        }

        public async Task Load()
        {
        }

        public async Task Unload()
        {
        }
    }
}
