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

        protected override IEnumerator StartLoading(Action onSceneLoadComplete)
        {
            _bundle.Load((assetBundle) =>
            {
                _scenetCoroutine = AsyncLoadScene(onSceneLoadComplete).StartAsCoroutine();
            });
            yield return 0;
        }

        private IEnumerator AsyncLoadScene(Action onSceneLoadingComplete)
        {
            var operation = SceneManager.LoadSceneAsync(Info.Name, Info.LoadSceneMode);
            yield return operation;
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            _bundle.Unload();
            if (_scenetCoroutine != null)
            {
                _scenetCoroutine.StopCoroutine();
                _scenetCoroutine = null;
            }
                
        }
    }


}
