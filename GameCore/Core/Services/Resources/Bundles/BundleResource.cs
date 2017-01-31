using System;
using System.Collections;
using GameCore.Core.UnityThreading;
using UnityEngine;

namespace GameCore.Core.Services.Resources.Bundles
{
    public class BundleResource: BaseResource<BundleInfo, AssetBundle>
    {
        private IBundleLoader _loader;
        private readonly bool _needCache;

        public BundleResource(BundleInfo info, string path) : base(info, path)
        {
            _needCache = info.Version != 0;
        }
        
        protected override IEnumerator LoadResource(Action<AssetBundle> onLoadComplete)
        {
            _loader = UnityEngine.Application.isEditor ? (IBundleLoader) new EditorBundleLoader() : (IBundleLoader) new WWWBundleLoader();
            yield return _loader.Load(Path, Info.Version).StartAsCoroutine();
            onLoadComplete(_loader.AssetBundle);
        }

        protected override void OnUnload(bool unloadDependences)
        {
            if (IsLoaded)
            {
                Asset.Unload(unloadDependences);
                _loader.Dispose();
            }
            UnityEngine.Resources.UnloadUnusedAssets();
        }
    }
}
