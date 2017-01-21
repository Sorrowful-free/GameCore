using System;
using System.Collections;
using UnityEngine;

namespace GameCore.Core.Services.Resources.Bundles
{
    public class BundleResource: BaseResource<BundleInfo, AssetBundle>
    {
        private WWW _www;
        private readonly bool _needCache;

        public BundleResource(BundleInfo info, string path) : base(info, path)
        {
            _needCache = info.Version != 0;
        }
        
        protected override IEnumerator LoadResource(Action<AssetBundle> onLoadComplete)
        {
            _www = _needCache ? WWW.LoadFromCacheOrDownload(Path, Info.Version) : new WWW(Path);
            yield return _www;
            onLoadComplete(_www.assetBundle);
        }

        protected override void OnUnload(bool unloadDependences)
        {
            if (IsLoaded)
            {
                Asset.Unload(unloadDependences);
                _www.Dispose();
            }
            UnityEngine.Resources.UnloadUnusedAssets();
        }
    }
}
