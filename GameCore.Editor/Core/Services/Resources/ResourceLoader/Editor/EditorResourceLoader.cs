using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCore.Core.Base.Dependency.Attributes;
using GameCore.Core.Services.Resources;
using GameCore.Core.Services.Resources.ResourceLoader;
using UnityEditor;

namespace GameCore.Editor.Core.Services.Resources.ResourceLoader.Editor
{
    [EditorDependency]
    public class EditorResourceLoader : BaseResourceLoader
    {
        private readonly string _assetBundlePath;
        private readonly BundleInfo _bundleInfo;
        public override bool IsFree => false;
        public override bool IsInitialize => true;

        public EditorResourceLoader(string assetBundlePath,BundleInfo bundleInfo)
        {
            _assetBundlePath = assetBundlePath;
            _bundleInfo = bundleInfo;
        }

        public async override Task<TAsset> LoadAsset<TAsset>(string assetName)
        {
            if (AssetDatabase.GetAssetPathsFromAssetBundle(_bundleInfo.Name).Contains(assetName))
            {
                return AssetDatabase.LoadAssetAtPath<TAsset>(assetName);
            }
            return null;
        }

        public async override Task UnloadAsset(string assetName)
        {
        }
    }
}
