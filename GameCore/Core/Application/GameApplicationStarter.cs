using System;
using GameCore.Core.Logging;
using UnityEngine;

namespace GameCore.Core.Application
{
    public class GameApplicationStarter : MonoBehaviour
    {
        private void Awake()
        {
            try
            {
                AsyncAwake();
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                throw ex;
            }
        }

        private async void AsyncAwake()
        {
            var startParameters = new GameApplicationStartParameters();
            ConfigureStartParameters(startParameters);
            await GameApplication.StartApplication(startParameters);
            OnInitializeComplete();
        }

        protected virtual void ConfigureStartParameters(GameApplicationStartParameters startParameters)
        {
            
        }

        protected virtual async void OnInitializeComplete()
        {
            
        }
    }
}
