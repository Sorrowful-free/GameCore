using System.Threading.Tasks;
using Core.Modules;
using Core.Runtime;
using Resources.Declaration;
using Resources.Declaration.Services;
using Resources.Runtime.Services;
using UnityEngine;

namespace Resources.Runtime
{
    public class ResourcesRuntimeModule : Module
    {
        public override async Task Initialize(ApplicationManager applicationManager)
        {
            await base.Initialize(applicationManager);
            if(ApplicationManager != null)
                await ApplicationManager.InitializeService<IResourcesService>(new ResourcesService());
        }

        public override async Task DeInitialize()
        {
            if(ApplicationManager != null)
                await ApplicationManager.DeInitializeService<IResourcesService>();
            await base.DeInitialize();
        }
    }
}
