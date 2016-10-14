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

        public void LoadScene(Action onSceneLoadComplete)
        {
            _loadCoroutine = StartLoading(onSceneLoadComplete).StartAsCoroutine();
        }
        
        protected abstract IEnumerator StartLoading(Action onSceneLoadComplete);

        protected virtual void OnUnload()
        {
            
        }

        public void Unload(Action onUnload)
        {
            _unloadCoroutine = AsyncUnload(onUnload).StartAsCoroutine();
        }

        private IEnumerator AsyncUnload(Action onUnload)
        {
            if (_loadCoroutine != null)
            {
                _loadCoroutine.StopCoroutine();
                _loadCoroutine = null;
            }
            yield return SceneManager.UnloadSceneAsync(Info.Name);
            OnUnload();
            onUnload.SafeInvoke();
            if (_unloadCoroutine != null)
            {
                _unloadCoroutine.StopCoroutine();
                _unloadCoroutine = null;
            }
                
        }

        public AwaitableOperation Unload()
        {
            return new AwaitableOperation(Unload);
        }
    }
}
