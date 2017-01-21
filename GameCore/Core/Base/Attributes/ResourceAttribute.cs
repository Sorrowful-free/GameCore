using System;
using System.Collections;
using System.Threading.Tasks;
using GameCore.Core.Base.Async;
using GameCore.Core.Extentions;
using GameCore.Core.UnityThreading;
using UnityEngine;

namespace GameCore.Core.Base.Attributes
{
    public class ResourceAttribute : Attribute,IGameObjectLoadAttribute
    {
        private readonly string _path;
        private GameObject _asset;
        public ResourceAttribute(string path)
        {
            _path = path;
        }

        public async Task<GameObject> LoadGameObject()
        {
            _asset = await new AwaitableOperation<GameObject>(AsyncLoad);
            return _asset;
        }

        private void AsyncLoad(Action<GameObject> callback)
        {
            UnityTask.MainThreadFactory.StartNew(() => InternalAsyncLoad(callback).StartAsCoroutine());
        }

        private IEnumerator InternalAsyncLoad(Action<GameObject> callback)
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
