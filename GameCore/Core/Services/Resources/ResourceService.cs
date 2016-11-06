using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameCore.Core.Application.Interfaces.Services;
using GameCore.Core.Extentions;
using GameCore.Core.Logging;
using GameCore.Core.Services.Resources.Assets;
using GameCore.Core.Services.Resources.Scenes;
using GameCore.Core.UnityThreading;
using UnityEngine;
using AssetBundleResource = GameCore.Core.Services.Resources.Bundles.AssetBundleResource;
using Object = UnityEngine.Object;

namespace GameCore.Core.Services.Resources
{
    public class ResourceService : IService
    {
        
        private Dictionary<int, IBaseResource<AssetInfo>> _assets = new Dictionary<int, IBaseResource<AssetInfo>>();
        private Dictionary<int, IBaseResource<BundleInfo>> _bundles = new Dictionary<int, IBaseResource<BundleInfo>>();
        private Dictionary<int, IBaseResource<SceneInfo>> _scenes = new Dictionary<int, IBaseResource<SceneInfo>>();

        public ReadOnlyCollection<int> AssetsIds { get { return _assets?.Keys?.ToList()?.AsReadOnly(); } }
        public ReadOnlyCollection<int> BundlesIds { get { return _bundles?.Keys?.ToList()?.AsReadOnly(); } }
        public ReadOnlyCollection<int> ScenesIds { get { return _scenes?.Keys?.ToList()?.AsReadOnly(); } }

        public ResourceTree ResourceTree { get; private set; }

        public async Task Initialize()
        {
            ResourceTree = new ResourceTree();
        }

        public async Task Deinitialize()
        {
            await UnityTask.MainThreadFactory.StartNew(Clear);
        }

        public BaseResource<AssetInfo, TAsset> GetAsset<TAsset>(int id) where TAsset:Object
        {
            var resource = default(IBaseResource<AssetInfo>);
            if (!_assets.TryGetValue(id, out resource))
            {
                var resInfo = ResourceTree.GetAssetInfo(id);
                var resPath = ResourceTree.GetAssetPath(id);
               
                if (resInfo.BundleId > 0)
                {
                    var assetBundle = GetBundle(resInfo.BundleId);
                    resource = new BundleResource<TAsset>(resInfo, resPath, assetBundle);
                }
                else
                {
                    resource = new LocalResource<TAsset>(resInfo, resPath);
                }
                _assets.Add(id,resource);
            }
            return (BaseResource<AssetInfo, TAsset>)resource;
        }
        
        public AssetBundleResource GetBundle(int id)
        {
            var bundle = default(IBaseResource<BundleInfo>);
            if (!_bundles.TryGetValue(id, out bundle))
            {
                var bundleInfo = ResourceTree.GetBundleInfo(id);
                var bundlePath = ResourceTree.GetBundlePath(id);
                bundle = new AssetBundleResource(bundleInfo, bundlePath);
                _bundles.Add(id,bundle);
            }
            return (AssetBundleResource)bundle;
        }

        public BaseSceneResource GetScene(int id)
        {
            var scene = default(IBaseResource<SceneInfo>);
            if (!_scenes.TryGetValue(id, out scene))
            {
                var sceneInfo = ResourceTree.GetSceneInfo(id);
                if (sceneInfo.BundleId > 0)
                {
                    scene = new BundleSceneResource(sceneInfo,GetBundle(sceneInfo.BundleId));
                }
                else
                {
                    scene = new LoadSceneResource(sceneInfo);
                }
                _scenes.Add(id, scene);
            }
            return (BaseSceneResource)scene;
        }

        public void DisposeAsset(int id)
        {
            if (_assets.ContainsKey(id))
            {
                _assets[id].Unload();
                _assets.Remove(id);
            }
        }

        public void DisposeBundle(int id)
        {
            if (_bundles.ContainsKey(id))
            {
                _bundles[id].Unload();
                _bundles.Remove(id);
            }
        }

        public void DisposeScene(int id)
        {
            if (_scenes.ContainsKey(id))
            {
                _scenes[id].Unload();
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
                var path = ResourceTree.GetBundlePath(bundlesId);
                var bundleInfo = ResourceTree.GetBundleInfo(bundlesId);
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
            var ids =
                await
                    UnityTask<int[]>.ThreadPoolFactory.StartNew(
                        () =>
                            BundlesIds.Where(
                                e => !ResourceTree.GetBundlePath(e).ToLower().Replace("\\", "/").Contains("file://"))
                                .ToArray());
            var percentPerBundle = 1.0f/(float) ids.Length;
            foreach (var bundlesId in ids)
            {
                await GetBundle(bundlesId).Load();
                progress += percentPerBundle;
                onProgress.SafeInvoke(progress);
            }
        }

       
    }
}
