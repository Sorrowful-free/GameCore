using System.Threading.Tasks;
using States.Declaration.Services.States;
using UnityEngine;

namespace ZombieIdler.Runtime.States
{
    public class PreloadState : IState
    {
        public void Dispose()
        {
            
        }

        public Task EnterState(params object[] enterArgs)
        {
            Debug.Log("preload");
            return Task.CompletedTask;
        }

        public Task ExitState()
        {
            return Task.CompletedTask;
        }
    }
}