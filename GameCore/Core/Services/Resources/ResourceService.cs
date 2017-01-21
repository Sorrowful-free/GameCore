﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameCore.Core.Application.Interfaces.Services;
using GameCore.Core.Extentions;
using GameCore.Core.Logging;
using GameCore.Core.Services.Resources.Assets;
using GameCore.Core.Services.Resources.Bundles;
using GameCore.Core.Services.Resources.Scenes;
using GameCore.Core.UnityThreading;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameCore.Core.Services.Resources
{
    public class ResourceService : IService
    {
        private ResourceTree _resourceTree;

        private Dictionary<int, IBaseResource<AssetInfo>> _assets = new Dictionary<int, IBaseResource<AssetInfo>>();
        private Dictionary<int, IBaseResource<BundleInfo>> _bundles = new Dictionary<int, IBaseResource<BundleInfo>>();
        private Dictionary<int, IBaseResource<SceneInfo>> _scenes = new Dictionary<int, IBaseResource<SceneInfo>>();
        

        public ReadOnlyCollection<int> AssetsIds { get { return _assets?.Keys?.ToList()?.AsReadOnly(); } }
        public ReadOnlyCollection<int> BundlesIds { get { return _bundles?.Keys?.ToList()?.AsReadOnly(); } }
        public ReadOnlyCollection<int> ScenesIds { get { return _scenes?.Keys?.ToList()?.AsReadOnly(); } }
        
        public async Task Initialize()
        {
            _resourceTree = new ResourceTree();
        }

        public async Task Deinitialize()
        {
            await UnityTask.MainThreadFactory.StartNew(Clear);
        }

        public void InitializeResourceTree(ResourcesInfo resourcesInfo)
        {
            _resourceTree.InitializeResourceData(resourcesInfo);
        }

        public BaseResource<AssetInfo, TAsset> GetAsset<TAsset>(int id) where TAsset:Object
        {
            var resource = default(IBaseResource<AssetInfo>);
            if (!_assets.TryGetValue(id, out resource))
            {
                var resInfo = _resourceTree.GetAssetInfo(id);
                var resPath = _resourceTree.GetAssetPath(id);
               
                if (resInfo.BundleId > 0)
                {
                    var assetBundle = GetBundle(resInfo.BundleId);
                    resource = new BundleAssetResource<TAsset>(resInfo, resPath, assetBundle);
                }
                else
                {
                    resource = new LocalAssetResource<TAsset>(resInfo, resPath);
                }
                _assets.Add(id,resource);
            }
            return (BaseResource<AssetInfo, TAsset>)resource;
        }
        
        public BundleResource GetBundle(int id)
        {
            var bundle = default(IBaseResource<BundleInfo>);
            if (!_bundles.TryGetValue(id, out bundle))
            {
                var bundleInfo = _resourceTree.GetBundleInfo(id);
                var bundlePath = _resourceTree.GetBundlePath(id);
                bundle = new BundleResource(bundleInfo, bundlePath);
                _bundles.Add(id,bundle);
            }
            return (BundleResource)bundle;
        }

        public BaseSceneResource GetScene(int id)
        {
            var scene = default(IBaseResource<SceneInfo>);
            if (!_scenes.TryGetValue(id, out scene))
            {
                var sceneInfo = _resourceTree.GetSceneInfo(id);
                if (sceneInfo.BundleId > 0)
                {
                    scene = new BundleSceneResource(sceneInfo,GetBundle(sceneInfo.BundleId));
                }
                else
                {
                    scene = new LocalSceneResource(sceneInfo);
                }
                _scenes.Add(id, scene);
            }
            return (BaseSceneResource)scene;
        }

        public void UnloadAsset(int id, bool unloadDependences = false)
        {
            if (_assets.ContainsKey(id))
            {
                _assets[id].Unload(unloadDependences);
                _assets.Remove(id);
            }
        }

        public void UnloadBundle(int id, bool unloadDependences = false)
        {
            if (_bundles.ContainsKey(id))
            {
                _bundles[id].Unload(unloadDependences);
                _bundles.Remove(id);
            }
        }

        public void UnloadScene(int id, bool unloadDependences = false)
        {
            if (_scenes.ContainsKey(id))
            {
                _scenes[id].Unload(unloadDependences);
                _scenes.Remove(id);
            }
        }

        public async Task Clear()
        {
            foreach (var resource in _assets.Values)
            {
                await resource.Unload();
            }
            _assets.Clear();

            foreach (var bundle in _bundles.Values)
            {
                await bundle.Unload();
            }
            _bundles.Clear();

            foreach (var scene in _scenes.Values)
            {
                await scene.Unload();
            }
            _scenes.Clear();
        }

        public bool HasUpdates()
        {
            var hasUpdate = false;
            foreach (var bundlesId in BundlesIds)
            {
                var path = _resourceTree.GetBundlePath(bundlesId);
                var bundleInfo = _resourceTree.GetBundleInfo(bundlesId);
                var needCheck = bundleInfo.Version != 0; // если файл не локальный то надо проверить на апдейт
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
            var ids =
                await
                    UnityTask<int[]>.ThreadPoolFactory.StartNew(
                        () =>
                            BundlesIds.Where(
                                e => _resourceTree.GetBundleInfo(e).Version != 0)
                                .ToArray());
            var percentPerBundle = 1.0f/(float) ids.Length;
            foreach (var bundlesId in ids)
            {
                await GetBundle(bundlesId).Load();
                progress += percentPerBundle;
                onProgress.SafeInvoke(progress);
            }
        }

        public async Task UnloadUnusedResources()
        {
            var unusedAssetsIds =
                await UnityTask<IEnumerable<int>>.ThreadPoolFactory.StartNew(
                    () => _assets.Where(e => e.Value.ReferenceCount <= 0).Select(e => e.Key));

            foreach (var assetsId in unusedAssetsIds)
            {
                UnloadAsset(assetsId);
            }
            var unusedScenesIds =
               await UnityTask<IEnumerable<int>>.ThreadPoolFactory.StartNew(
                   () => _scenes.Where(e => e.Value.ReferenceCount <= 0).Select(e => e.Key));
            foreach (var scenesId in unusedScenesIds)
            {
                UnloadScene(scenesId);
            }
            var unusedBundlesIds =
                await UnityTask<IEnumerable<int>>.ThreadPoolFactory.StartNew(
                    () => _bundles.Where(e => e.Value.ReferenceCount <= 0).Select(e => e.Key));
            foreach (var bundlesId in unusedBundlesIds)
            {
                UnloadBundle(bundlesId);
            }
            UnityEngine.Resources.UnloadUnusedAssets();
            GC.Collect();
        }
    }
}
