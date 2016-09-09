using System;
using System.Threading;
using System.Threading.Tasks;
using GameCore.Core;
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
         //   UnitySynchronizationContext.MakeUnity();
         //   await Task.Delay(2500);
            GetComponent<Text>().text = $"olololo {DateTime.Now}";

            //SynchronizationContext.SetSynchronizationContext(UnitySynchronizationContext.Default);
            await Task.Delay(2500);
            GetComponent<Text>().text = $"olololo {DateTime.Now}";
            
            
            
            var test = await DependencyInjector.GetDependency<ITest>();
            GetComponent<Text>().text = $"olololo {test.Lol} {nameof(test)}";
        //    UnitySynchronizationContext.MakeDefault();

        }

        protected override void Update()
        {
            base.Update();
            transform.eulerAngles += Vector3.forward;
        }
    }

    public interface ITest
    {
        int Lol { get; }
    }

    public class MyTest :ITest
    {
        public int Lol { get { return 36; } }
    }
}
