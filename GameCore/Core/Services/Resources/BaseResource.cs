using System;
using System.Collections;
using System.Runtime.CompilerServices;
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

        public void StartLoading(Action<TAsset> onLoadComplete)
        {
            if (Asset != null && IsLoaded)
            {
                onLoadComplete.SafeInvoke(Asset);
                return;
            }

            InternalOnLoadComplete += onLoadComplete;
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

        public void StopLoading()
        {
            if (_coroutine != null)
            {
                _coroutine.StopCoroutine();
                _coroutine = null;
            }
        }

        public void Dispose()
        {
            OnDisposed();
            StopLoading();
            Asset = null;
            IsLoaded = false;
        }
       

        protected abstract IEnumerator LoadResource(Action<TAsset> onLoadComplete);
        protected abstract void OnDisposed();

        public IAwaiter<TAsset> GetAwaiter()
        {
            return new ResourceAwaiter<TInfo,TAsset>(this);
        }
    }
}