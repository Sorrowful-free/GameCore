﻿using System;
using System.Collections;
using System.Threading.Tasks;
using GameCore.Core.Base.Async;
using GameCore.Core.Base.Attributes;
using GameCore.Core.Extentions;
using GameCore.Core.UnityThreading;
using UnityEngine;

namespace GameCore.Core.Services.UI
{
    public class UIServiceResourceAttribute : Attribute,IGameObjectLoadAttribute
    {
        private readonly string _path;
        private GameObject _asset;
        public UIServiceResourceAttribute(string path)
        {
            _path = path;
        }

        public async Task<GameObject> LoadGameObject()
        {
           return await new AwaitableOperation<GameObject>((go) => AsyncLoad(go).StartAsCoroutine());
        }

        private IEnumerator AsyncLoad(Action<GameObject> callback)
        {
            var res = UnityEngine.Resources.LoadAsync<GameObject>(_path);
            yield return res;
            callback.SafeInvoke((GameObject)res.asset);
        }

        public async Task UnloadGameObject()
        {
            if(_asset != null)
                UnityEngine.Resources.UnloadAsset(_asset);
        }
    }
}