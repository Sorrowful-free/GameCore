using System;
using System.Collections;
using UnityEngine;

namespace GameCore.Core.Services.Resources.Bundles
{
    public class LocalAssetBundleResource:BaseAssetBundleResource
    {
        public LocalAssetBundleResource(BundleInfo info, string path) : base(info, path)
        {
        }
        
        protected override IEnumerator LoadResource(Action<AssetBundle> onLoadComplete)
        {
            var resourceOperation = UnityEngine.Resources.LoadAsync(Path);
            yield return resourceOperation;
            var textAsset = (TextAsset)resourceOperation.asset;
            var assetBundleOperation = AssetBundle.LoadFromMemoryAsync(textAsset.bytes);
            yield return assetBundleOperation;
            onLoadComplete(assetBundleOperation.assetBundle);
            UnityEngine.Resources.UnloadAsset(textAsset);
        }
    }
}
