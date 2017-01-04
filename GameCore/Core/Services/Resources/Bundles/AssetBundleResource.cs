using System;
using System.Collections;
using UnityEngine;

namespace GameCore.Core.Services.Resources.Bundles
{
    public class AssetBundleResource: BaseResource<BundleInfo, AssetBundle>
    {
        private WWW _www;
        private readonly bool _needCache;

        public AssetBundleResource(BundleInfo info, string path) : base(info, path)
        {
            _needCache = info.Version != 0;
        }
        
        protected override IEnumerator LoadResource(Action<AssetBundle> onLoadComplete)
        {
            _www = _needCache ? WWW.LoadFromCacheOrDownload(Path, Info.Version) : new WWW(Path);
            yield return _www;
            onLoadComplete(_www.assetBundle);
        }

        protected override void OnUnload()
        {
            if (IsLoaded)
            {
                Asset.Unload(true);
                _www.Dispose();
            }
        }
    }
}
