using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameCore.Core.Services.Resources.Bundles
{
    public interface IBundleLoader : IDisposable
    {
        AssetBundle AssetBundle { get; }
        IEnumerator Load(string url, int version);
    }
}
