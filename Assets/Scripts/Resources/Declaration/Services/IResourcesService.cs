using System.Threading.Tasks;
using Core.Runtime.Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;


namespace Resources.Declaration.Services
{
    public interface IResourcesService : IService
    {
        Task<Object> Load(string assetName);
        Task<TAsset> Load<TAsset>(string assetName) where TAsset : Object;
        void Unload(Object asset);

        Task<Object> Instantiate(string assetName, Vector3 position, Quaternion rotation);
        Task<Object> Instantiate(string assetName);

        Task<TInstance> Instantiate<TInstance>(string assetName, Vector3 position, Quaternion rotation) where TInstance : Object;
        Task<TInstance> Instantiate<TInstance>(string assetName) where TInstance : Object;
            
        void Destroy(Object instance);

        void UnloadAllUnusedResources();

        Task<Scene> LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single);
        Task<Scene> LoadScene(string sceneName);
        void UnloadScene(string sceneName);
    }
}