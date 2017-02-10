using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using GameCore.Core.Base.Async;
using GameCore.Core.Base.Dependency.Attributes;
using GameCore.Core.Extentions;
using GameCore.Core.Logging;
using GameCore.Core.UnityThreading;
using UnityEngine;

namespace GameCore.Core.Services.Resources.ResourceLoader.Bundle
{
    public class BundleResourceLoader : BaseResourceLoader
    {
        private readonly string _assetBundlePath;
        private readonly BundleInfo _bundleInfo;

        private AssetBundle _bundle;
        public override bool IsFree => Resources.All(e => e.Value.IsLoaded && e.Value.ReferenceCount <= 0);
        public override bool IsInitialize => _bundle != null;
        private Coroutine _loadingBundle;

        public BundleResourceLoader(string assetBundlePath,BundleInfo bundleInfo)
        {
            _assetBundlePath = assetBundlePath;
            _bundleInfo = bundleInfo;
        }

        public async override Task<TAsset> LoadAsset<TAsset>(string assetName)
        {
            var resource = default(IResource);
            if (!Resources.TryGetValue(assetName, out resource))
            {
                resource = new BundleResource(_bundle.LoadAssetAsync<TAsset>(assetName));
                Resources.Add(assetName, resource);
            }
            resource.ReferenceCount++;
            return (TAsset)await resource.WaitLoading();
        }

        public async override Task UnloadAsset(string assetName)
        {
            var resource = default(IResource);
            if (Resources.TryGetValue(assetName, out resource))
            {
                --resource.ReferenceCount;
            }
        }

        public async override Task WaitInitialize()
        {
            await base.WaitInitialize();
            if (_loadingBundle == null)
            {
                _loadingBundle = LoadAssetBundle(a=>_bundle = a).StartAsCoroutine();
            }
            await new WaitWhileOperation(() => !IsInitialize);
        }

        private IEnumerator LoadAssetBundle(Action<AssetBundle> onLoadDone)
        {
            var www = _bundleInfo.Version <= 0
                ? new WWW(_assetBundlePath)
                : WWW.LoadFromCacheOrDownload(_assetBundlePath, _bundleInfo.Version);
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Log.Error("www cant download asset bundle {0}\n{1}",_assetBundlePath,www.error);
            }
            onLoadDone.SafeInvoke(www.assetBundle);
            www.Dispose();
        }

        public async override Task Deinitialize()
        {
            await base.Deinitialize();
            _bundle.Unload(false);
            _bundle = null;
        }
    }
}
