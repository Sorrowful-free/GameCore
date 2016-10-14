using System;
using System.Runtime.CompilerServices;
using GameCore.Core.Base.Async;
using Object = UnityEngine.Object;

namespace GameCore.Core.Services.Resources
{
    public interface IBaseResource<TInfo>
    {
        TInfo Info { get; }
        bool IsLoaded { get; }

        void Unload(Action onUnload);
        AwaitableOperation Unload();
    }
    public interface IResource<TInfo, TAsset>:IBaseResource<TInfo> 
        where TAsset :Object
    {
        
        TAsset Asset { get; }
        string Path { get; }
        void Load(Action<TAsset> onLoad);
        AwaitableOperation<TAsset> Load();


    }
}
