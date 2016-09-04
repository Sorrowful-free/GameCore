using System;
using System.Runtime.CompilerServices;
using Object = UnityEngine.Object;

namespace GameCore.Core.Services.Resources
{
    public interface IBaseResource<TInfo>:IDisposable
    {
        TInfo Info { get; }
        string Path { get; }
        bool IsLoaded { get; }
        
    }
    public interface IResource<TInfo, TAsset>:IBaseResource<TInfo> , IAwaitable<TAsset>
        where TAsset :Object
    {
        
        TAsset Asset { get; }

        void StartLoading(Action<TAsset> onLoadComplete);
        void StopLoading();

    }
}
