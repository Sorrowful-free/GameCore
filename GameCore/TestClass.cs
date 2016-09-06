﻿using System.Threading.Tasks;
using GameCore.Core.Services.Resources;
using GameCore.Core.UnityThreading;
using UnityEngine;

namespace GameCore
{
    class TestClass
    {
        public async Task Test()
        {
            var resourceService = default(ResourceService);
            if (resourceService.HasUpdates())
            {
                await resourceService.LoadAllAssetBundles((p) => Debug.Log(p));
            }
            var go = await resourceService.GetAsset<GameObject>(0);
            new Task(() =>
            {
                Debug.Log("ololo");
            }).Start(UnityTaskScheduler.Instance);
        }
    }
}
