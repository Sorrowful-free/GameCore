using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

namespace GameCore.Core.Services.Resources
{
    public class ResourceTree
    {
        private Dictionary<int, DirectoryInfo> _resourceDirectories;
        private Dictionary<int, AssetInfo> _assets;
        private Dictionary<int, BundleInfo> _bundles;
        private Dictionary<int, SceneInfo> _scenes;

        public void InitializeResourceData(ResourcesInfo resourceInfo)
        {
            Clear();
            _resourceDirectories = resourceInfo.ResourceDirectories.ToDictionary(e => e.Id);
            _assets = resourceInfo.Assets.ToDictionary(e => e.Id);
            _bundles = resourceInfo.Bundles.ToDictionary(e => e.Id);
            _scenes = resourceInfo.Scenes.ToDictionary(e => e.Id);

        }

        public void AddResourceData(ResourcesInfo resourceInfo)
        {
            if (_resourceDirectories != null)
            {
                foreach (var pair in resourceInfo.ResourceDirectories)
                {
                    _resourceDirectories.Add(pair.Id,pair);
                }
            }
           
            if (_assets != null)
            {
                foreach (var pair in resourceInfo.Assets)
                {
                    _assets.Add(pair.Id, pair);
                }
            }

            if (_bundles != null)
            {
                foreach (var pair in resourceInfo.Bundles)
                {
                    _bundles.Add(pair.Id, pair);
                }
            }
        }

        public void Clear()
        {
            if (_resourceDirectories != null)
            {
                _resourceDirectories.Clear();
            }
            if (_assets != null)
            {
                _assets.Clear();
            }
            if (_bundles != null)
            {
                _bundles.Clear();
            }
        }

        public string GetDirectoryPath(int id)
        {
            var dir = default(DirectoryInfo);
            var directoriPath = "";
            if (_resourceDirectories.TryGetValue(id, out dir))
            {
                directoriPath =  Path.Combine(GetDirectoryPath(dir.ParentId), dir.Name );
            }
            return directoriPath;
        }

        public string GetAssetPath(int id)
        {
            var asset = default(AssetInfo);
            if (_assets.TryGetValue(id, out asset))
            {
                return Path.Combine(GetDirectoryPath(asset.DirectoryId),asset.Name);
            }
            return "";
        }

        public string GetBundlePathByAssetId(int id)
        {
            var asset = default(AssetInfo);
            if (_assets.TryGetValue(id, out asset))
            {
                return GetBundlePath(asset.BundleId);
            }
            return "";
        }

        public string GetBundlePath(int id)
        {
            var bundle = default(BundleInfo);
            if (_bundles.TryGetValue(id, out bundle))
            {
                return Path.Combine(GetDirectoryPath(bundle.DirectoryId), bundle.Name);
            }
            return "";
        }

        public AssetInfo GetAssetInfo(int id)
        {
            var resInfo = default(AssetInfo);
            _assets.TryGetValue(id, out resInfo);
            return resInfo;
        }

        public BundleInfo GetBundleInfo(int id)
        {
            var bundleInfo = default(BundleInfo);
            _bundles.TryGetValue(id, out bundleInfo);
            return bundleInfo;
        }

        public SceneInfo GetSceneInfo(int id)
        {
            var sceneInfo = default(SceneInfo);
            _scenes.TryGetValue(id, out sceneInfo);
            return sceneInfo;
        }
    }

    //TODO add serialization
    public struct ResourcesInfo 
    {
        public List<DirectoryInfo> ResourceDirectories;
        public List<AssetInfo> Assets;
        public List<BundleInfo> Bundles;
        public List<SceneInfo> Scenes;
    }
    
    public struct DirectoryInfo
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
    }

    public struct AssetInfo
    {
        public int Id { get; set; }
        public int DirectoryId { get; set; }
        public int BundleId { get; set; }
        public string Name { get; set; }
    }

    public struct BundleInfo
    {
        public int Id { get; set; }
        public int DirectoryId { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
    }

    public struct SceneInfo
    {
        public int Id { get; set; }
        public int BundleId { get; set; }
        public string Name { get; set; }
        public LoadSceneMode LoadSceneMode { get; set; }
    }
}
