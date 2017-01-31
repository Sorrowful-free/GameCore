using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameCore.Core.Services.Resources.Bundles
{
    internal class EditorBundleLoader : IBundleLoader
    {
        public AssetBundle AssetBundle { get; }
        public IEnumerator Load(string url, int version)
        {
            
        }

        public void Dispose()
        {
            
        }

        
    }
}
