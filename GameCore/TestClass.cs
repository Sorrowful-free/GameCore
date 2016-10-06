using System;
using System.Threading.Tasks;
using GameCore.Core.Base;
using GameCore.Core.Services.UI;
using GameCore.Core.Services.UI.Layers.Info;
using GameCore.Core.UnityThreading;
using UnityEngine;

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
        protected override async void Awake()
        {
            Debug.Log("start");
            var service = gameObject.AddComponent<TestUIService>();
           // await service.Initialize();
            UnitySynchronizationContext.MakeUnity();

            await Task.Factory.StartNew(() =>
                {
                    throw new ApplicationException("ololosh");
                });
            

            
            
            Debug.Log("end");
        }
        
      
    }
}
