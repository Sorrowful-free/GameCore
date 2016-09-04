using System;
using System.Collections;
using Object = UnityEngine.Object;

namespace GameCore.Core.Services.Resources.Assets
{
    public class LocalResource<TAsset> : BaseResource<AssetInfo,TAsset> where TAsset:Object
    {
        public LocalResource(AssetInfo info, string path) : base(info, path)
        {
        }

        protected override void OnDisposed()
        {
            if (IsLoaded)
            {
                UnityEngine.Resources.UnloadAsset(Asset);
            }
        }

        protected override IEnumerator LoadResource(Action<TAsset> onLoadComplete)
        {
            var operation = UnityEngine.Resources.LoadAsync(Path);
            yield return operation; //TODO надо проверить будет ли так работать
            onLoadComplete((TAsset)operation.asset);
        }
        
    }
}
