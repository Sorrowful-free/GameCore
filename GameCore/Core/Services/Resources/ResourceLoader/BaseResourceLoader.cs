using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCore.Core.Base.Async;
using GameCore.Core.Extentions;
using GameCore.Core.UnityThreading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace GameCore.Core.Services.Resources.ResourceLoader
{
    public abstract class BaseResourceLoader: IResourceLoader
    {
        private Coroutine _checkUnusedResources;
        protected Dictionary<string,IResource> Resources = new Dictionary<string, IResource>();
        public abstract bool IsFree { get; }
        public abstract bool IsInitialize { get; }
        public abstract Task<TAsset> LoadAsset<TAsset>(string assetName) where TAsset : Object;
        public abstract Task UnloadAsset(string assetName);

        public async Task<Scene> LoadScene(string sceneName)
        {
            await WaitInitialize();
            await SceneManager.LoadSceneAsync(sceneName).ToAwaitable();
            return SceneManager.GetSceneByName(sceneName);
        }

        public async Task UnLoadScene(string sceneName)
        {

            await new AwaitableOperation((c) =>
            {
                var onUnloaded = (default(UnityAction<Scene>));
                onUnloaded = (scene) =>
                {
                    if (scene.name == sceneName)
                    {
                        SceneManager.sceneUnloaded -= onUnloaded;
                        c.SafeInvoke();
                    }
                };
                SceneManager.sceneUnloaded += onUnloaded;
                SceneManager.UnloadScene(sceneName);
            });
        }

        public async virtual Task WaitInitialize()
        {
            StartCheckingUnusedResources();
        }

        private void StartCheckingUnusedResources()
        {
            StopCheckingUnusedResources();
            CheckUnusedResources().StartAsCoroutine();
        }

        private IEnumerator CheckUnusedResources()
        {
            yield return new WaitForSeconds(5);
            var unusedResources = Resources.Where(e => e.Value.IsLoaded && e.Value.ReferenceCount <= 0).Select(e=>e.Key);

            foreach (var resource in unusedResources)
            {
                var task = UnloadAsset(resource);
                yield return new WaitWhile(() => !task.IsCompleted);
            }
            yield return UnityEngine.Resources.UnloadUnusedAssets();
            GC.Collect();
        }

        private void StopCheckingUnusedResources()
        {
            if (_checkUnusedResources != null)
            {
                _checkUnusedResources.StopCoroutine();
            }
        }
        public async virtual Task Deinitialize()
        {
            StopCheckingUnusedResources();
        }
    }
}
