using System;
using GameCore.Core.Extentions;
using System.Runtime.CompilerServices;
using Object = UnityEngine.Object;

namespace GameCore.Core.Services.Resources
{
    public struct ResourceAwaiter<TInfo, TAsset> : IAwaiter<TAsset> where TAsset : Object
    {
        private readonly IResource<TInfo, TAsset> _resource;

        public bool IsCompleted
        {
            get { return _resource.IsLoaded; }
        }

        public ResourceAwaiter(IResource<TInfo, TAsset> resource)
        {
            _resource = resource;
        }

        public TAsset GetResult()
        {
            return _resource.Asset;
        }

        public void OnCompleted(Action continuation)
        {
            _resource.StartLoading((asset)=> continuation.SafeInvoke());
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            _resource.StartLoading((asset) => continuation.SafeInvoke());
        }
    }
}
