using System;
using System.Collections;
using System.Runtime.CompilerServices;
using GameCore.Core.Base.Async;
using GameCore.Core.Extentions;
using GameCore.Core.UnityThreading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore.Core.Services.Resources.Scenes
{
    public abstract class BaseSceneResource : IBaseResource<SceneInfo>
    {
        public SceneInfo Info { get; private set; }
        public bool IsLoaded { get; protected set; }

        private Coroutine _loadCoroutine;
        private Coroutine _unloadCoroutine;

        public BaseSceneResource(SceneInfo info)
        {
            Info = info;
        }

        public void Load(Action onSceneLoadComplete)
        {
            if (_loadCoroutine == null)
                _loadCoroutine = StartLoading(onSceneLoadComplete).StartAsCoroutine();
        }

        public AwaitableOperation Load()
        {
            return new AwaitableOperation(Load);
        }
        
        protected abstract IEnumerator StartLoading(Action onSceneLoadComplete);

        protected virtual void OnUnload()
        {
            
        }

        public void Unload(Action onUnload)
        {
            if(_unloadCoroutine == null)
                _unloadCoroutine = AsyncUnload(onUnload).StartAsCoroutine();
        }

        public AwaitableOperation Unload()
        {
            return new AwaitableOperation(Unload);
        }

        private IEnumerator AsyncUnload(Action onUnload)
        {
            if (_loadCoroutine != null)
            {
                _loadCoroutine.StopCoroutine();
                _loadCoroutine = null;
            }
            yield return 0;
            SceneManager.UnloadScene(Info.Name);
            yield return 0;
            OnUnload();
            onUnload.SafeInvoke();
            if (_unloadCoroutine != null)
            {
                _unloadCoroutine.StopCoroutine();
                _unloadCoroutine = null;
            }
                
        }
    }
}
