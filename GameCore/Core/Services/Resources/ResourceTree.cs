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
                directoriPath =  GetDirectoryPath(dir.ParentId) +dir.Directory ;
            }
            else
            {
                directoriPath = dir.Directory;
            }
            return directoriPath + Path.DirectorySeparatorChar;
        }

        public string GetAssetPath(int id)
        {
            var asset = default(AssetInfo);
            if (_assets.TryGetValue(id, out asset))
            {
                return GetDirectoryPath(asset.DirectoryId) + asset.Name;
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
                return GetDirectoryPath(bundle.DirectoryId)  + bundle.Name;
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
        public int Id;
        public int ParentId;
        public string Directory;
    }

    public struct AssetInfo
    {
        public int Id;
        public int DirectoryId;
        public int BundleId;
        public string Name;
    }

    public struct BundleInfo
    {
        public int Id;
        public int DirectoryId;
        public int Version;
        public string Name;
        public int Size;
    }

    public struct SceneInfo
    {
        public int Id;
        public int BundleId;
        public string Name;
        public LoadSceneMode LoadSceneMode;
    }
}
