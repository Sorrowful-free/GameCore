using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace GameCore.Core.Services.Resources.ResourceLoader
{
    public interface IResourceLoader
    {
        bool IsFree { get; }
        bool IsInitialize { get; }

        Task<TAsset> LoadAsset<TAsset>(string assetName) where TAsset : Object;

        Task UnloadAsset(string assetName);

        Task<Scene> LoadScene(string sceneName);
        Task UnLoadScene(string sceneName);

        Task WaitInitialize();
        Task Deinitialize();
    }
}
