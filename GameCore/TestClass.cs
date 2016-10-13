using System;
using System.Diagnostics;
using System.Threading.Tasks;
using GameCore.Core.Application;
using GameCore.Core.Base;
using GameCore.Core.Services.UI;
using GameCore.Core.Services.UI.Layers.Info;
using GameCore.Core.UnityThreading;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GameCore
{
    public class TestUIService : BaseUIService<int>
    {
        public override async Task Initialize()
        {
            await AddLayer(0, new UILayerInfo
            {
                RenderMode = RenderMode.ScreenSpaceCamera
            });
            await AddLayer(1, new UILayerInfo
            {
                RenderMode = RenderMode.ScreenSpaceOverlay
            });
            await AddLayer(2, new UILayerInfo
            {
                RenderMode = RenderMode.WorldSpace
            });
        }

        public override async Task Deinitialize()
        {
            
        }
    }
    public class TestClass : BaseMonoBehaviour
    {
        protected async override void Awake()
        {
            Debug.Log("start");
            UnitySynchronizationContext.MakeUnity();
            try
            {
                await UnityTask.MainThreadFactory.StartNew(() =>
                {
                    throw new Exception("ololsh");
                });
            }
            catch (Exception ex)
            {
                
                Debug.Log("asdasdasd"+ex);
            }
           
          
            StackTracePrint();





            // await service.Initialize();


        }

        private void StackTracePrint()
        {
            Debug.Log(StackTraceUtility.ExtractStackTrace());
        }
        private async Task TestExecute()
        {

            await Task.Delay(0);
            Debug.Log("exception");
        }
        
      
    }
}
