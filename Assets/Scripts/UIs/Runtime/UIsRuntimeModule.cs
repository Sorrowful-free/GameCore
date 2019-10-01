using System.Threading.Tasks;
using Core.Modules;
using Core.Runtime;
using Resources.Declaration.Services;
using UIs.Declaration.Services;
using UnityEngine;

namespace UIs.Runtime
{
    public class UIsRuntimeModule : Module
    {
        public override async Task Initialize(ApplicationManager applicationManager)
        {
            await base.Initialize(applicationManager);
            var resources = ApplicationManager.GetService<IResourcesService>();
            await ApplicationManager.InitializeService<IUIsService>(
                (await resources.Instantiate<GameObject>("Services/UIsService")).GetComponent<UIsService>());
        }

        public override async Task DeInitialize()
        {
            await ApplicationManager.DeInitializeService<IUIsService>();
            await base.DeInitialize();
        }
    }
}