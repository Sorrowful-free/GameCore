using System;
using System.Collections;
using System.Runtime.CompilerServices;
using GameCore.Core.Base.Async;
using GameCore.Core.Extentions;
using GameCore.Core.UnityThreading;
using UnityEngine;

namespace GameCore.Core.Services.Resources
{
    public abstract class BaseResource<TInfo, TAsset>:IResource<TInfo,TAsset> where TAsset : UnityEngine.Object
    {

        private event Action<TAsset> InternalOnLoadComplete;
        
        public TInfo Info { get; private set; }
        public string Path { get; private set; }

        public bool IsLoaded { get; protected set; }

        public TAsset Asset { get; private set; }

        private Coroutine _coroutine;

        protected BaseResource(TInfo info,string path)
        {
            Info = info;
            Path = path;
        }

        public void Load(Action<TAsset> onLoad)
        {
            if (Asset != null && IsLoaded)
            {
                onLoad.SafeInvoke(Asset);
                return;
            }

            InternalOnLoadComplete += onLoad;
            if (_coroutine == null)
                _coroutine = LoadResource((asset) =>
                {
                    Asset = asset;
                    InternalOnLoadComplete.SafeInvoke(Asset);
                    InternalOnLoadComplete.ClearAllHandlers();
                    IsLoaded = true;
                    StopLoading();
                }).StartAsCoroutine();
        }

        public AwaitableOperation<TAsset> Load()
        {
            return new AwaitableOperation<TAsset>(Load);
        }

        private void StopLoading()
        {
            if (_coroutine != null)
            {
                _coroutine.StopCoroutine();
                _coroutine = null;
            }
        }
        
        public void Unload(Action onUnload)
        {
            OnUnload();
            StopLoading();
            Asset = null;
            IsLoaded = false;
            onUnload.SafeInvoke();
        }

        public AwaitableOperation Unload()
        {
            return new AwaitableOperation(Unload);
        }
        
        protected abstract IEnumerator LoadResource(Action<TAsset> onLoadComplete);
        protected abstract void OnUnload();

        public static implicit operator TAsset(BaseResource<TInfo, TAsset> resource)
        {
            return resource.Asset;
        }
    }
}