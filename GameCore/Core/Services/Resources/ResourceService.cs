﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameCore.Core.Extentions;
using GameCore.Core.Services.Resources.Assets;
using GameCore.Core.Services.Resources.Bundles;
using GameCore.Core.UnityThreading;
using UnityEngine;
using AssetBundleResource = GameCore.Core.Services.Resources.Bundles.AssetBundleResource;
using Object = UnityEngine.Object;

namespace GameCore.Core.Services.Resources
{
    public class ResourceService /*: IService*/
    {
        
        private Dictionary<int, IBaseResource<AssetInfo>> _assets = new Dictionary<int, IBaseResource<AssetInfo>>();
        private Dictionary<int, IResource<BundleInfo, AssetBundle>> _bundles = new Dictionary<int, IResource<BundleInfo, AssetBundle>>();

        public ReadOnlyCollection<int> AssetsIds { get { return new ReadOnlyCollection<int>(_assets.Keys.ToList());} }
        public ReadOnlyCollection<int> BundlesIds { get { return new ReadOnlyCollection<int>(_bundles.Keys.ToList()); } }

        public ResourceTree ResourceTree { get; private set; }

        public void Initialize()
        {
            ResourceTree = new ResourceTree();
        }

        public IResource<AssetInfo,TAsset> GetAsset<TAsset>(int id) where TAsset:Object
        {
            var resource = default(IBaseResource<AssetInfo>);
            if (!_assets.TryGetValue(id, out resource))
            {
                var resInfo = ResourceTree.GetAssetInfo(id);
                var resPath = ResourceTree.GetAssetPath(id);
               
                if (resInfo.BundleId >= 0)
                {
                    var assetBundle = GetBundle(resInfo.BundleId);
                    resource = new AssetBundleResource<TAsset>(resInfo, resPath, assetBundle);
                }
                else
                {
                    resource = new LocalResource<TAsset>(resInfo, resPath);
                }
                _assets.Add(id,resource);
            }
            return (IResource<AssetInfo,TAsset>)resource;
        }
        
        public IResource<BundleInfo,AssetBundle> GetBundle(int id)
        {
            var bundle = default(IResource<BundleInfo, AssetBundle>);
            if (!_bundles.TryGetValue(id, out bundle))
            {
                var bundleInfo = ResourceTree.GetAssetBundleInfo(id);
                var bundlePath = ResourceTree.GetAssetBundlePath(id);
                bundle = new AssetBundleResource(bundleInfo, bundlePath);
                _bundles.Add(id,bundle);
            }
            return bundle;
        }

        public void DisposeAsset(int id)
        {
            _assets[id].Dispose();
        }

        public void DisposeBundle(int id)
        {
            _bundles[id].Dispose();
        }
        public void Clear()
        {
            foreach (var resource in _assets.Values)
            {
                resource.Dispose();
            }
            _assets.Clear();

            foreach (var bundle in _bundles.Values)
            {
                bundle.Dispose();
            }
            _bundles.Clear();
        }

        public void Dispose()
        {
            Clear();
        }

        public bool HasUpdates()
        {
            var hasUpdate = false;
            foreach (var bundlesId in BundlesIds)
            {
                var path = ResourceTree.GetAssetBundlePath(bundlesId);
                var bundleInfo = ResourceTree.GetAssetBundleInfo(bundlesId);
                var needCheck = !path.ToLower().Replace("\\", "/").Contains("file://"); // если файл не локальный то надо проверить на апдейт
                if (needCheck)
                {
                    hasUpdate = hasUpdate || !Caching.IsVersionCached(path, bundleInfo.Version);
                }
            }
            return hasUpdate;
        }

        public async Task LoadAllAssetBundles(Action<float> onProgress)
        {
            var progress = 0.0f;
            var ids = await Task<int[]>.Factory.StartNew(() => BundlesIds.Where( e => !ResourceTree.GetAssetBundlePath(e).ToLower().Replace("\\", "/").Contains("file://")).ToArray()
#if UNITY_WEBGL
            ,
            CancellationToken.None,
            TaskCreationOptions.None, 
            UnityTaskScheduler.Instance
#endif
            );
            var percentPerBundle = 1.0f/(float) ids.Length;
            foreach (var bundlesId in ids)
            {
                await GetBundle(bundlesId);
                progress += percentPerBundle;
                onProgress.SafeInvoke(progress);
            }
#if !UNITY_WEBGL
            await Task.Delay(100);
#endif
        }
    }
}
