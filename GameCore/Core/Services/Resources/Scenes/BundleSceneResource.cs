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
        private readonly AssetBundleResource _bundle;
        private Coroutine _scenetCoroutine;
        public BundleSceneResource(SceneInfo info,AssetBundleResource bundle) : base(info)
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

        protected override void OnDispose()
        {
            base.OnDispose();
            _bundle.Dispose();
            if (_scenetCoroutine != null)
            {
                _scenetCoroutine.StopCoroutine();
                _scenetCoroutine = null;
            }
                
        }
    }


}
