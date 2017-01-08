using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ResourcesInfo(ReadOnlyCollection<DirectoryInfo> resourceDirectories, ReadOnlyCollection<BundleInfo> bundles, ReadOnlyCollection<AssetInfo> assets, ReadOnlyCollection<SceneInfo> scenes)
        {
            ResourceDirectories = resourceDirectories;
            Bundles = bundles;
            Assets = assets;
            Scenes = scenes;
        }

        public ReadOnlyCollection<DirectoryInfo> ResourceDirectories { get; }
        
        public ReadOnlyCollection<BundleInfo> Bundles { get; }
        public ReadOnlyCollection<AssetInfo> Assets { get; }
        public ReadOnlyCollection<SceneInfo> Scenes { get; }
    }
    
    public struct DirectoryInfo
    {
        public DirectoryInfo(int id, int parentId, string name)
        {
            Id = id;
            ParentId = parentId;
            Name = name;
        }

        public int Id { get; }
        public int ParentId { get; }
        public string Name { get; }
    }

    public struct AssetInfo
    {
        public AssetInfo(int id, int directoryId, int bundleId, string name)
        {
            Id = id;
            DirectoryId = directoryId;
            BundleId = bundleId;
            Name = name;
        }

        public int Id { get; }
        public int DirectoryId { get; }
        public int BundleId { get; }
        public string Name { get; }
    }

    public struct BundleInfo
    {
        public BundleInfo(int id, int directoryId, int version, string name, int size)
        {
            Id = id;
            DirectoryId = directoryId;
            Version = version;
            Name = name;
            Size = size;
        }

        public int Id { get; }
        public int DirectoryId { get; }
        public int Version { get; }
        public string Name { get; }
        public int Size { get;  }
    }

    public struct SceneInfo
    {
        public SceneInfo(int id, int bundleId, string name, LoadSceneMode loadSceneMode)
        {
            Id = id;
            BundleId = bundleId;
            Name = name;
            LoadSceneMode = loadSceneMode;
        }

        public int Id { get; }
        public int BundleId { get; }
        public string Name { get; }
        public LoadSceneMode LoadSceneMode { get; }
    }
}
