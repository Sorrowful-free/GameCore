using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GameCore.Core.Application.Interfaces.Services;
using GameCore.Core.Base.Dependency;
using GameCore.Core.Extentions;
using GameCore.Core.Services.Resources.ResourceLoader;
using GameCore.Core.UnityThreading;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameCore.Core.Services.Resources
{
    public class ResourceService : IService
    {
        private ResourceTree _resourceTree;
        private Dictionary<int, IResourceLoader> _resourceLoaders;
        private ResourceLoaderFactory _resourceLoaderFactory;
        public bool IsNeedDownload => _resourceTree.IsNeedDownload;

        public async Task Initialize()
        {
            _resourceTree = new ResourceTree();
            _resourceLoaderFactory = new ResourceLoaderFactory(_resourceTree);
        }

        public async Task Deinitialize()
        {
          
        }

        public void InitializeResourceTree(ResourcesInfo resourcesInfo)
        {
            _resourceTree.InitializeResourceData(resourcesInfo);
        }

        public async Task<TAsset> LoadAsset<TAsset>(int id) where TAsset:Object
        {
            var assetInfo = _resourceTree.GetAssetInfo(id);
            var loader = GetResourceLoader(assetInfo.BundleId);
            var assetPath = _resourceTree.GetAssetPath(id);
            return await loader.LoadAsset<TAsset>(assetPath);
        }

        public async Task<IEnumerable<TAsset>> LoadAssets<TAsset>(params int[] ids) where TAsset : Object
        {
            return await Task.WhenAll(ids.Select(e => LoadAsset<TAsset>(e)));
        }

        public async Task UnloadAsset(int id)
        {
            var assetInfo = _resourceTree.GetAssetInfo(id);
            var loader = GetResourceLoader(assetInfo.BundleId);
            var assetPath = _resourceTree.GetAssetPath(id);
            loader.UnloadAsset(assetPath);
        }

        public async Task UnloadAssets(params int[] ids)
        {
            foreach (var id in ids)
            {
                UnloadAsset(id);
            }
        }
        
        public async Task DownloadAllAssetsBundles(Action<float> onProgress)
        {
            var ids = _resourceTree.NeedLoadBundlesIds;
            var count = ids.Count;
            var index = 0;
            foreach (var id in ids)
            {
                var loader = GetResourceLoader(id);
                await loader.WaitInitialize();
                onProgress.SafeInvoke(((float)index++)/(float)count,ActionInvokationType.MainThread);
            }
            onProgress.SafeInvoke(1,ActionInvokationType.MainThread);
        }

        private IResourceLoader GetResourceLoader(int id)
        {
            var resourceLoader = default(IResourceLoader);
            if(!_resourceLoaders.TryGetValue(id, out resourceLoader))
            {
                resourceLoader = _resourceLoaderFactory.CreateInstance(id);
                _resourceLoaders.Add(id,resourceLoader);
            }
            return resourceLoader;
        }
    }
}
