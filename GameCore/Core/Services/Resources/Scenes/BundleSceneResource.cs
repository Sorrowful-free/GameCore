using System;
using System.Collections;
using GameCore.Core.Services.Resources.Bundles;
using GameCore.Core.UnityThreading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore.Core.Services.Resources.Scenes
{
    public class BundleSceneResource : BaseSceneResource
    {
        private readonly IResource<BundleInfo, AssetBundle> _bundle;
        private Coroutine _scenetCoroutine;
        public BundleSceneResource(SceneInfo info,IResource<BundleInfo,AssetBundle> bundle) : base(info)
        {
            _bundle = bundle;
        }

        protected override IEnumerator LoadScene(Action<Scene> onSceneLoadComplete)
        {
            _bundle.Load((assetBundle) =>
            {
                _scenetCoroutine = AsyncLoadScene(onSceneLoadComplete).StartAsCoroutine();
            });
            yield return 0;
        }

        private IEnumerator AsyncLoadScene(Action<Scene> onSceneLoadingComplete)
        {
            var operation = SceneManager.LoadSceneAsync(Info.Name, Info.LoadSceneMode);
            Scene = SceneManager.GetSceneByName(Info.Name);
            yield return operation;
        }

        protected override void OnUnload(bool unloadDependences)
        {
            if(unloadDependences)
                _bundle.Unload(unloadDependences);
            if (_scenetCoroutine != null)
            {
                _scenetCoroutine.StopCoroutine();
                _scenetCoroutine = null;
            }
            base.OnUnload(unloadDependences);
        }
    }


}
