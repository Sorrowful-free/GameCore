using System;
using System.Threading.Tasks;
using Core.Runtime;

namespace Core.Modules
{
    
    public abstract class Module : IDisposable
    {
        public ApplicationManager ApplicationManager { get; private set; }
        public virtual Task Initialize(ApplicationManager applicationManager)
        {
            ApplicationManager = applicationManager;
            return Task.CompletedTask;
        }

        public virtual void OnApplicationFocus(bool hasFocus)
        {
            
        }

        public virtual void OnApplicationPause(bool pauseStatus)
        {
            
        }

        public virtual Task DeInitialize()
        {
            return Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            ApplicationManager = null;
        }
    }
}