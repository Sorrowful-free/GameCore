using System.Threading.Tasks;
using Core.Runtime;
using Resources.Runtime;
using States.Runtime;
using UIs.Runtime;

namespace ZombieIdler.Runtime
{
    public class ZombieIdlerApplicationManagerDelegate : ApplicationManagerDelegate
    {
        public ZombieIdlerApplicationManagerDelegate(ApplicationManager applicationManager) : base(applicationManager)
        {
        }

        public override async Task OnApplicationStart()
        {
            await base.OnApplicationStart();
            await ApplicationManager.InitializeModule(new ResourcesRuntimeModule());
            await ApplicationManager.InitializeModule(new UIsRuntimeModule()); 
            await ApplicationManager.InitializeModule(new StatesRuntimeModule());
            await ApplicationManager.InitializeModule(new ZombieIdleRuntimeModule());
        }
    }
}
