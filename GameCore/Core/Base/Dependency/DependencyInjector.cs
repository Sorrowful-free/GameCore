using System;
using System.Linq;
using Assets.Scripts.Core.Extentions;
using GameCore.Core.Base.Dependency.Attributes;
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

        public static TBaseDependency GetDependency<TBaseDependency>(Type type)
        {
            var implementedType = GetDependencyType(type);
            var dependency = (TBaseDependency)Activator.CreateInstance(implementedType);
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

        public static Type GetDependencyType(Type interfaceType)
        {
            return AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(e => e.GetTypes())
                    .FirstOrDefault(t => interfaceType.IsAssignableFrom(t) 
                    && !t.IsInterface 
                    && t.GetAttribute<PlatformDependencAttribute>()?.Platform == UnityEngine.Application.platform);
        }
    }
}
