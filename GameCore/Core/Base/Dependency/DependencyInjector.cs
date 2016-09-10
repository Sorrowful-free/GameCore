using System;
using System.Linq;
using UnityEngine;

namespace GameCore.Core
{
    public static class DependencyInjector
    {
        public static TDependency GetDependency<TDependency>()
        {
            var type = typeof(TDependency);
            var implementedType = GetDependencyType(type);
            var dependency = (TDependency)Activator.CreateInstance(implementedType);
            return dependency;
        }

        public static TDependency AddDependencyComponent<TDependency>(GameObject gameobject)
        {
            var type = typeof(TDependency);
            var implementedType = GetDependencyType(type);
            var component = gameobject.AddComponent(implementedType);
            return (TDependency)(object)component;
        }

        public static TDependency GetDependencyComponent<TDependency>(GameObject gameobject)
        {
            var type = typeof(TDependency);
            var implementedType = GetDependencyType(type);
            var component = gameobject.GetComponent(implementedType);
            return (TDependency)(object)component;
        }

        public static TDependency TryGetOrAddDependencyComponent<TDependency>(GameObject gameobject)
        {
            var type = typeof(TDependency);
            var implementedType = GetDependencyType(type);
            var component = gameobject.GetComponent(implementedType);
            if (component == null)
            {
                component = gameobject.AddComponent(implementedType);
            }
            return (TDependency)(object)component;
        }

        private static Type GetDependencyType(Type interfaceType)
        {
            var allTypes =
                AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(e => e.GetTypes())
                    .Where(t => interfaceType.IsAssignableFrom(t) && !t.IsInterface);
            if (allTypes.Count() > 1)
            {
                //TODO Logging
            }

            if (allTypes.Count() == 0)
            {
                //TODO Logging
                return null;
            }
            var implementedType = allTypes.FirstOrDefault();
            return implementedType;
        }
    }
}
