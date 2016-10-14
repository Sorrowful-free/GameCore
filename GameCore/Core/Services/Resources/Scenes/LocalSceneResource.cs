using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace GameCore.Core.Services.Resources.Scenes
{
    public class LocalSceneResource : BaseSceneResource
    {
        public LocalSceneResource(SceneInfo info) : base(info)
        {
        }

        protected override IEnumerator StartLoading(Action callback)
        {
            var operation = SceneManager.LoadSceneAsync(Info.Name, Info.LoadSceneMode);
            yield return operation;
        }

        
    }
}
