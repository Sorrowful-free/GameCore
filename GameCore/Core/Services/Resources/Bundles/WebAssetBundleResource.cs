using System;
using System.Collections;
using UnityEngine;

namespace GameCore.Core.Services.Resources.Bundles
{
    public class WebAssetBundleResource:BaseAssetBundleResource
    {
        private WWW _www;
        public WebAssetBundleResource(BundleInfo info, string path) : base(info, path)
        {
        }
        
        protected override IEnumerator LoadResource(Action<AssetBundle> onLoadComplete)
        {
            _www = WWW.LoadFromCacheOrDownload(Path, Info.Version);
            yield return _www;
            onLoadComplete(_www.assetBundle);
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();
            _www.Dispose();
        }
    }
}
