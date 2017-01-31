using System.Collections;
using UnityEngine;

namespace GameCore.Core.Services.Resources.Bundles
{
    internal class WWWBundleLoader : IBundleLoader
    {
        public AssetBundle AssetBundle { get; }

        public IEnumerator Load(string url, int version)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        
    }
}
