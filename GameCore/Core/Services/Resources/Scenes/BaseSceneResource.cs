using System;
using System.Collections;
using System.Runtime.CompilerServices;
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

        private Coroutine _coroutine;

        public BaseSceneResource(SceneInfo info)
        {
            Info = info;
        }

        public void LoadScene(Action onSceneLoadComplete)
        {
            _coroutine = StartLoading(onSceneLoadComplete).StartAsCoroutine();
        }
        
        protected abstract IEnumerator StartLoading(Action onSceneLoadComplete);

        protected virtual void OnDispose()
        {
            
        }
        public void Dispose()
        {
            if (_coroutine != null)
            {
                _coroutine.StopCoroutine();
                _coroutine = null;
            }
            SceneManager.UnloadScene(Info.Name);
            OnDispose();
        }

        public IAwaiter GetAwaiter()
        {
            return new CallbackAwaiter(LoadScene);
        }


    }
}
