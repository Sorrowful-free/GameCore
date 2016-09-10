using UnityEngine;

namespace GameCore.Core.Base.Dependency
{
    public static class DependencyInjectionExtention
    {

        public static TDependency AddDependencyComponent<TDependency>(this GameObject gameobject)
        {
            return DependencyInjector.AddDependencyComponent<TDependency>(gameobject);
        }

        public static TDependency GetDependencyComponent<TDependency>(this GameObject gameobject)
        {
            return DependencyInjector.GetDependencyComponent<TDependency>(gameobject);
        }

        public static TDependency TryGetOrAddDependencyComponent<TDependency>(GameObject gameobject)
        {
            return DependencyInjector.TryGetOrAddDependencyComponent<TDependency>(gameobject);
        }
    }
}
