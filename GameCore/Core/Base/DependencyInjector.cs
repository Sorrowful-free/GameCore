using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace GameCore.Core
{
    public static class DependencyInjector
    {
        public static async Task<TDependency> GetDependency<TDependency>()
        {
            return await Task<TDependency>.Factory.StartNew(() =>
            {
                var type = typeof(TDependency);
                if (!type.IsInterface)
                {
                    //TODO Logging
                }

                var allTypes =
                    AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(e => e.GetTypes())
                        .Where(t => type.IsAssignableFrom(t) && !t.IsInterface);
                if (allTypes.Count() > 1)
                {
                    //TODO Logging
                }

                if (allTypes.Count() == 0)
                {
                    return default(TDependency);
                }
                var implementedType = allTypes.FirstOrDefault();
                var dependency = (TDependency)Activator.CreateInstance(implementedType);
                return dependency;
            });
        }
    }
}
