using System;
using System.Collections;
using Object = UnityEngine.Object;

namespace GameCore.Core.Services.Resources.Assets
{
    public class LocalAssetResource<TAsset> : BaseResource<AssetInfo,TAsset> where TAsset:Object
    {
        public LocalAssetResource(AssetInfo info, string path) : base(info, path)
        {
        }

        protected override void OnUnload(bool unloadDependences)
        {
            if (IsLoaded)
            {
                UnityEngine.Resources.UnloadAsset(Asset);
            }
            UnityEngine.Resources.UnloadUnusedAssets();
        }

        protected override IEnumerator LoadResource(Action<TAsset> onLoadComplete)
        {
            var operation = UnityEngine.Resources.LoadAsync(Path);
            yield return operation; //TODO надо проверить будет ли так работать
            onLoadComplete((TAsset)operation.asset);
        }
        
    }
}
