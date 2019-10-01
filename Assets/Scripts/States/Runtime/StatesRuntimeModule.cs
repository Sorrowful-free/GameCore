using System.Threading.Tasks;
using Core.Modules;
using Core.Runtime;
using States.Declaration.Services;
using States.Runtime.Services;

namespace States.Runtime
{
    public class StatesRuntimeModule : Module
    {
        public override async Task Initialize(ApplicationManager applicationManager)
        {
            await base.Initialize(applicationManager);
            if(ApplicationManager != null)
                await ApplicationManager.InitializeService<IStatesService>(new StatesService());
        }

        public override async Task DeInitialize()
        {
            if(ApplicationManager != null)
                await ApplicationManager.DeInitializeService<IStatesService>();
            await base.DeInitialize();
        }
    }
}
