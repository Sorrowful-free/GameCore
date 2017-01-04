using System;
using System.Collections;
using GameCore.Core.UnityThreading;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameCore.Core.Services.Resources.Assets
{
    public class BundleResource<TAsset> : BaseResource<AssetInfo,TAsset> where TAsset : Object
    {
        private readonly IResource<BundleInfo, AssetBundle> _bundle;
        private Coroutine _coroutine;
        public BundleResource(AssetInfo info, string path, IResource<BundleInfo,AssetBundle> bundle) : base(info, path)
        {
            _bundle = bundle;
        }

        protected override IEnumerator LoadResource(Action<TAsset> onLoadComplete)
        {
            _bundle.Load((bundle) =>
            {
               _coroutine = LoadResourceFromAssetBundle(bundle, onLoadComplete).StartAsCoroutine();
            });
            yield return 0;
        }
        
        private IEnumerator LoadResourceFromAssetBundle(AssetBundle assetBundle,Action<TAsset> onLoadComplete)
        {
            var assetOperation = assetBundle.LoadAssetAsync(Path);
            yield return assetOperation;
            onLoadComplete((TAsset)assetOperation.asset);
        }

        protected override void OnUnload()
        {
            _bundle.Unload();
        }
    }
}
