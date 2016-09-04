using UnityEngine;

namespace GameCore.Core.Services.Resources.Bundles
{
    public abstract class BaseAssetBundleResource : BaseResource<BundleInfo,AssetBundle>
    {
        public BaseAssetBundleResource(BundleInfo info, string path) : base(info, path)
        {
        }
        protected override void OnDisposed()
        {
            if (IsLoaded)
            {
                Asset.Unload(true);
            }
        }
    }
}
