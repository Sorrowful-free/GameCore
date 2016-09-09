using System;
using System.Threading;
using System.Threading.Tasks;
using GameCore.Core.Base;
using GameCore.Core.Services.Resources;
using GameCore.Core.UnityThreading;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore
{
    public class TestClass : BaseMonoBehaviour
    {
        protected async override void Awake()
        {
            base.Awake();
            await Test();
            Debug.Log("done");
        }

        public async Task Test()
        {
            SynchronizationContext.SetSynchronizationContext(UnitySynchronizationContext.Unity);
            await Task.Delay(2500);
            GetComponent<Text>().text = $"olololo {DateTime.Now}";

            //SynchronizationContext.SetSynchronizationContext(UnitySynchronizationContext.Default);
            await Task.Delay(2500);
            GetComponent<Text>().text = $"olololo {DateTime.Now}";

            var rs = default(ResourceService);

            SynchronizationContext.SetSynchronizationContext(UnitySynchronizationContext.Unity);
            var mySprite = await rs.GetAsset<Sprite>(90);

        }

        protected override void Update()
        {
            base.Update();
            transform.eulerAngles += Vector3.forward;
        }
    }
}
