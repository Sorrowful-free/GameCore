using System.Threading.Tasks;
using Core.Runtime.Async;
using Resources.Declaration.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Resources.Runtime.Services
{
    public class ResourcesService : IResourcesService
    {
        public void Dispose()
        {
        }

        public Task Initialize()
        {
            return Task.CompletedTask;
        }

        public Task DeInitialize()
        {
            return Task.CompletedTask;
        }

        public async Task<Object> Load(string assetName)
        {
            var asyncOperation = await UnityEngine.Resources.LoadAsync<Object>(assetName);
            return asyncOperation.asset;
        }

        public async Task<TAsset> Load<TAsset>(string assetName) where TAsset : Object
        {
            var asyncOperation = await UnityEngine.Resources.LoadAsync<TAsset>(assetName);
            return (TAsset) asyncOperation.asset;
        }

        public void Unload(Object asset)
        {
            UnityEngine.Resources.UnloadAsset(asset);
        }

        public async Task<Object> Instantiate(string assetName, Vector3 position = default(Vector3),
            Quaternion rotation = default(Quaternion))
        {
            return Object.Instantiate(await Load(assetName), position, rotation);
        }

        public async Task<Object> Instantiate(string assetName)
        {
            return Object.Instantiate(await Load(assetName));
        }

        public async Task<TInstance> Instantiate<TInstance>(string assetName, Vector3 position, Quaternion rotation) where TInstance : Object
        {
            return (TInstance) await Instantiate(assetName, position, rotation);
        }

        public async Task<TInstance> Instantiate<TInstance>(string assetName) where TInstance : Object
        {
            return (TInstance) await Instantiate(assetName);
        }

        public void Destroy(Object instance)
        {
            Object.Destroy(instance);
        }

        public void UnloadAllUnusedResources()
        {
            UnityEngine.Resources.UnloadUnusedAssets();
        }

        public async Task<Scene> LoadScene(string sceneName, LoadSceneMode loadSceneMode)
        {
            var sceneRequest = await SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            return SceneManager.GetSceneByName(sceneName);
        }

        public async Task<Scene> LoadScene(string sceneName)
        {
            var sceneRequest = await SceneManager.LoadSceneAsync(sceneName);
            return SceneManager.GetSceneByName(sceneName);
        }

        public void UnloadScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }
}