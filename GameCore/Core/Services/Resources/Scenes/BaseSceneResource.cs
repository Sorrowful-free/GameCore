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
        private event Action<Scene> InternalOnLoadComplete;
        public SceneInfo Info { get; private set; }
        public Scene Scene { get; protected set; }
        public bool IsLoaded { get; protected set; }

        public int ReferenceCount { get; private set; }

        private Coroutine _loadCoroutine;

        public BaseSceneResource(SceneInfo info)
        {
            Info = info;
        }

        public void Load(Action<Scene> onSceneLoadComplete)
        {
            ReferenceCount++;
            if (Scene != default(Scene) && IsLoaded)
            {
                onSceneLoadComplete.SafeInvoke(Scene);
                return;
            }

            InternalOnLoadComplete += onSceneLoadComplete;
            if (_loadCoroutine == null)
                _loadCoroutine = LoadScene((scene) =>
                {
                    Scene = scene;
                    InternalOnLoadComplete.SafeInvoke(Scene);
                    InternalOnLoadComplete.ClearAllHandlers();
                    IsLoaded = true;
                    StopLoading();
                }).StartAsCoroutine();
        }

        public AwaitableOperation<Scene> Load()
        {
            return new AwaitableOperation<Scene>(Load);
        }

        protected abstract IEnumerator LoadScene(Action<Scene> onSceneLoadComplete);

        private void StopLoading()
        {
            if (_loadCoroutine != null)
            {
                _loadCoroutine.StopCoroutine();
                _loadCoroutine = null;
            }
                
        }

        public void Unload(Action onUnload,bool unloadDependences = false)
        {
            StopLoading();
            SceneManager.UnloadScene(Info.Name);
            OnUnload(unloadDependences);
            onUnload.SafeInvoke();
            ReferenceCount--;
        }

        public AwaitableOperation Unload(bool unloadDependences = false)
        {
            return new AwaitableOperation((c)=>Unload(c,unloadDependences));
        }

        protected virtual void OnUnload(bool unloadDependences)
        {
            UnityEngine.Resources.UnloadUnusedAssets();
        }
    }
}
