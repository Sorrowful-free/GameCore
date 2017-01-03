using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCore.Core.Base;
using GameCore.Core.Base.Async;

namespace GameCore.Core.Application
{
    public abstract class GamePreloader : BaseMonoBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
        }

        private async Task AsyncAwake()
        {
            await PreInitialize();
            await Initialize();
            await PostInitialize();
            UnityAsync.Destroy(gameObject);
        }

        protected abstract Task PreInitialize();
        protected abstract Task Initialize();
        protected abstract Task PostInitialize();

    }
}
