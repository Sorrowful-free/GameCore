using System;
using System.Collections;
using GameCore.Core.UnityThreading;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameCore.Core.Services.Resources.Assets
{
    public class BundleResource<TAsset> : BaseResource<AssetInfo,TAsset> where TAsset : Object
    {
        private readonly IResource<BundleInfo, AssetBundle> _assetBundleResource;
        private Coroutine _coroutine;
        public BundleResource(AssetInfo info, string path, IResource<BundleInfo,AssetBundle> assetBundleResource) : base(info, path)
        {
            _assetBundleResource = assetBundleResource;
        }

        protected override IEnumerator LoadResource(Action<TAsset> onLoadComplete)
        {
            _assetBundleResource.StartLoading((bundle) =>
            {
               _coroutine = LoadResourceFromAssetBundle(bundle, onLoadComplete).StartAsCoroutine();
            });
            yield return 0;
        }

        protected override void OnDisposed()
        {
            _assetBundleResource.Dispose();
        }
        
        private IEnumerator LoadResourceFromAssetBundle(AssetBundle assetBundle,Action<TAsset> onLoadComplete)
        {
            var assetOperation = assetBundle.LoadAssetAsync(Path);
            yield return assetOperation;
            onLoadComplete((TAsset)assetOperation.asset);
        }
    }
}
