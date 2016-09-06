﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameCore.Core.Services.Resources
{
    public class ResourceTree
    {
        private Dictionary<int, DirectoryInfo> _resourceDirectories;
        private Dictionary<int, AssetInfo> _assets;
        private Dictionary<int, BundleInfo> _bundles;

        public void InitializeResourceData(ResourcesInfo resourceInfo)
        {
            Clear();
            _resourceDirectories = resourceInfo.ResourceDirectories.ToDictionary(e => e.Id);
            _assets = resourceInfo.Resources.ToDictionary(e => e.Id);
            _bundles = resourceInfo.Assets.ToDictionary(e => e.Id);
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
                foreach (var pair in resourceInfo.Resources)
                {
                    _assets.Add(pair.Id, pair);
                }
            }

            if (_bundles != null)
            {
                foreach (var pair in resourceInfo.Assets)
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

        public string GetAssetBundlePathByAssetId(int id)
        {
            var asset = default(AssetInfo);
            if (_assets.TryGetValue(id, out asset))
            {
                return GetAssetBundlePath(asset.BundleId);
            }
            return "";
        }

        public string GetAssetBundlePath(int id)
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
            if (_assets.TryGetValue(id, out resInfo))
            {
                return resInfo;
            }
            return resInfo;

        }

        public BundleInfo GetAssetBundleInfo(int id)
        {
            var bundleInfo = default(BundleInfo);
            if (_bundles.TryGetValue(id, out bundleInfo))
            {
                return bundleInfo;
            }
            return bundleInfo;
        }
    }

    //TODO add serialization
    public struct ResourcesInfo 
    {
        public List<DirectoryInfo> ResourceDirectories;
        public List<AssetInfo> Resources;
        public List<BundleInfo> Assets;
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
    }
}
