using System.Threading.Tasks;

namespace Core.Runtime
{
    public abstract class ApplicationManagerDelegate
    {
        public ApplicationManager ApplicationManager { get; private set; }
        public ApplicationManagerDelegate(ApplicationManager applicationManager)
        {
            ApplicationManager = applicationManager;
        }
        
        public virtual Task OnApplicationStart()
        {
            return Task.CompletedTask;
        }

        public void OnApplicationFocus(bool hasFocus)
        {
            
        }

        public void OnApplicationPause(bool pauseStatus)
        {
            
        }

        public virtual void OnApplicationQuit()
        {
        }

        

    }
}