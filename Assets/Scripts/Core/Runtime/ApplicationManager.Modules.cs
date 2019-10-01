using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Modules;

namespace Core.Runtime
{
    public partial class ApplicationManager
    {
        private List<Module> _modules = new List<Module>();

        public async Task InitializeModule(Module moduleImplementation)
        {
            _modules.Add(moduleImplementation);
            await moduleImplementation.Initialize(this);
        }

        private async Task DeInitializeModules()
        {
            while (_modules.Count>0)
            {
                var module = _modules.First();
                if (_modules.Remove(module))
                {
                    await module.DeInitialize();
                    module.Dispose();    
                }
            }
        }

        private void ModulesOnApplicationFocus(bool hasFocus)
        {
            foreach (var module in _modules.ToArray())
            {
                module.OnApplicationFocus(hasFocus);
            }
        }

        private void ModulesOnApplicationPause(bool pauseStatus)
        {
            foreach (var module in _modules.ToArray())
            {
                module.OnApplicationPause(pauseStatus);
            }
        }

        private async void ModulesOnApplicationQuit()
        {
            await DeInitializeModules();
        }
    }
}