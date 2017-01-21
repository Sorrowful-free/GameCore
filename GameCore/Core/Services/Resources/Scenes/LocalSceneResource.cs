﻿using System;
using System.Collections;
using GameCore.Core.Extentions;
using UnityEngine.SceneManagement;

namespace GameCore.Core.Services.Resources.Scenes
{
    public class LocalSceneResource : BaseSceneResource
    {
        public LocalSceneResource(SceneInfo info) : base(info)
        {
        }

        protected override IEnumerator LoadScene(Action<Scene> callback)
        {
            var operation = SceneManager.LoadSceneAsync(Info.Name, Info.LoadSceneMode);
            yield return operation;
            Scene = SceneManager.GetSceneByName(Info.Name);
            callback.SafeInvoke(Scene);
        }
    }
}
