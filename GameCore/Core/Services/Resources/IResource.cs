using System;
using GameCore.Core.Base.Async;
using Object = UnityEngine.Object;

namespace GameCore.Core.Services.Resources
{
    public interface IBaseResource<TInfo>
    {
        TInfo Info { get; }
        bool IsLoaded { get; }
        int ReferenceCount { get; }
        void Unload(Action onUnload,bool unloadDependences = false);
        AwaitableOperation Unload(bool unloadDependences = false);
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
