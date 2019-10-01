using System.Threading.Tasks;
using Core.Modules;
using Core.Runtime;
using States.Declaration.Services;
using ZombieIdler.Runtime.States;

namespace ZombieIdler.Runtime
{
    public class ZombieIdleRuntimeModule : Module
    {
        public async override Task Initialize(ApplicationManager applicationManager)
        {
            await base.Initialize(applicationManager);
            await ApplicationManager.GetService<IStatesService>().SetState<PreloadState>();
        }
    }
}