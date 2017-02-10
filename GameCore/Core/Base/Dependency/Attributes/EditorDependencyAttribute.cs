using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameCore.Core.Base.Dependency.Attributes
{
    [AttributeUsage(AttributeTargets.Interface|AttributeTargets.Class)]
    public class EditorDependencyAttribute : Attribute, IDependencyAttribute
    {
        public bool IsCanCreate => true;
    }
}
