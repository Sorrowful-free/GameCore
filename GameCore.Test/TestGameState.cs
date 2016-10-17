using System.Threading.Tasks;
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
            Debug.LogError("enter state");
        }

        public async Task Load()
        {
        }

        public async Task Unload()
        {
        }
    }
}
