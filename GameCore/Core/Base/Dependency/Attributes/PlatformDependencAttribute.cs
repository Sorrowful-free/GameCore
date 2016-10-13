using System;
using UnityEngine;

namespace GameCore.Core.Base.Dependency.Attributes
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Interface)]
    public class PlatformDependencAttribute : Attribute
    {
        public RuntimePlatform Platform { get; private set; }
        public PlatformDependencAttribute(RuntimePlatform platform)
        {
            Platform = platform;
        }
    }
}
