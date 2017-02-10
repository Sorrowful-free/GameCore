using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace GameCore.Core.Base.Dependency.Attributes
{
    public interface IDependencyAttribute
    {
        bool IsCanCreate { get; }
    }
}
