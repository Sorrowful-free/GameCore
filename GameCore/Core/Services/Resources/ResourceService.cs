using System.Collections.Generic;
using GameCore.Core.Services.Resources.Assets;
using GameCore.Core.Services.Resources.Bundles;
using UnityEngine;

namespace GameCore.Core.Services.Resources
{
    public class ResourceService /*: IService*/
    {
        
        private Dictionary<int, IBaseResource<AssetInfo>> _assets = new Dictionary<int, IBaseResource<AssetInfo>>();
        private Dictionary<int, IResource<BundleInfo, AssetBundle>> _bundles = new Dictionary<int, IResource<BundleInfo, AssetBundle>>();

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
                var resInfo = ResourceTree.GetResourceInfo(id);
                var resPath = ResourceTree.GetResourcePath(id);
               
                if (resInfo.BundleId >= 0)
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
            return (IResource<AssetInfo,TAsset>)resource;
        }
        
        public IResource<BundleInfo,AssetBundle> GetBundle(int id)
        {
            var bundle = default(IResource<BundleInfo, AssetBundle>);
            if (!_bundles.TryGetValue(id, out bundle))
            {
                var bundleInfo = ResourceTree.GetBundleInfo(id);
                var bundlePath = ResourceTree.GetAssetPath(id);
                if (bundlePath.ToLower().StartsWith("resources://"))
                {
                    bundle = new LocalAssetBundleResource(bundleInfo, bundlePath);
                }
                else
                {
                    bundle = new WebAssetBundleResource(bundleInfo, bundlePath);
                }
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
    }
}
