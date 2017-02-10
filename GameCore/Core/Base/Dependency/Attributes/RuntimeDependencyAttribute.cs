using System;

namespace GameCore.Core.Base.Dependency.Attributes
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Interface)]
    public class RuntimeDependencyAttribute : Attribute, IDependencyAttribute
    {
        public bool IsCanCreate => !UnityEngine.Application.isEditor;
    }
}
