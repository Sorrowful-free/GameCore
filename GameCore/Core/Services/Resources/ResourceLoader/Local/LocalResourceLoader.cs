using System.Threading.Tasks;

namespace GameCore.Core.Services.Resources.ResourceLoader.Local
{
    public class LocalResourceLoader : BaseResourceLoader
    {
        public override bool IsFree => false;
        public override bool IsInitialize => true;
        public override async Task<TAsset> LoadAsset<TAsset>(string assetName)
        {
            var resource = default(IResource);
            if (!Resources.TryGetValue(assetName, out resource))
            {
                resource = new LocalResource(UnityEngine.Resources.LoadAsync<TAsset>(assetName));
                Resources.Add(assetName,resource);
            }
            resource.ReferenceCount++;
            return (TAsset)await resource.WaitLoading();
        }

        public override async Task UnloadAsset(string assetName)
        {
            var resource = default(IResource);
            if (Resources.TryGetValue(assetName, out resource))
            {
                if (--resource.ReferenceCount <= 0 && resource.IsLoaded)
                {
                    UnityEngine.Resources.UnloadAsset(resource.Asset);
                }
            }
            UnityEngine.Resources.UnloadUnusedAssets();
        }
    }
}
