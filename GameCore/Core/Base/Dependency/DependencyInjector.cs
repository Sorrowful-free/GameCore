using System;
using System.Linq;
using GameCore.Core.Base.Dependency.Attributes;
using GameCore.Core.Extentions;
using UnityEngine;

namespace GameCore.Core.Base.Dependency
{
    public static class DependencyInjector
    {
        public static TDependency GetDependency<TDependency>(params object[] ctorParams)
        {
            var type = typeof(TDependency);
            var implementedType = GetDependencyType(type);
            var dependency = (TDependency)Activator.CreateInstance(implementedType, ctorParams);
            return dependency;
        }

        public static TBaseDependency GetDependency<TBaseDependency>(Type type, params object[] ctorParams)
        {
            var implementedType = GetDependencyType(type);
            var dependency = (TBaseDependency)Activator.CreateInstance(implementedType, ctorParams);
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
            var type = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(e => e.GetTypes())
                .FirstOrDefault(t => interfaceType.IsAssignableFrom(t)
                                     && !t.IsInterface
                                     && CheckDependency(t));
            if (type == null)
            {
                throw new AggregateException($"not found dependence for type {interfaceType.Name}");
            }
            return type;
        }

        private static bool CheckDependency(Type type)
        {
            var attributes = type.GetAttributesByInterface<IDependencyAttribute>();

            if (UnityEngine.Application.isEditor)
            {
                return attributes.Any(e => e is EditorDependencyAttribute);
            }
            var attribute = attributes.FirstOrDefault(e => e is PlatformDependencAttribute || e is RuntimeDependencyAttribute);
            if (attribute != null)
                return attribute.IsCanCreate;

            return false;
        }
    }
}
