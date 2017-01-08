using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCore.Core.Base;
using GameCore.Core.Base.Async;
using GameCore.Core.Logging;

namespace GameCore.Core.Application
{
    public abstract class GamePreloader : BaseMonoBehaviour
    {

        protected override void Start()
        {
            base.Start();
            try
            {
                AsyncStart();
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }
        }

        private async Task AsyncStart()
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
