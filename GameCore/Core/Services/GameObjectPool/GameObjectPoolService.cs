using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameCore.Core.Application.Interfaces.Services;
using GameCore.Core.Base;
using GameCore.Core.Base.Async;
using GameCore.Core.Logging;
using GameCore.Core.UnityThreading;
using UnityEngine;

namespace GameCore.Core.Services.GameObjectPool
{
    public class GameObjectPoolService : BaseMonoBehaviour, IService
    {
        private List<PoolableGameObject> _pollableObjects = new List<PoolableGameObject>();
        private GameTimer _timer = new GameTimer(0.5f);
        public async Task Initialize()
        {
            _timer.Start(CheckObjects);
            Log.Info("GameObjectPoolService initialize");
        }

        public async Task Deinitialize()
        {
            await UnityTask.MainThreadFactory.StartNew(Clear);
            Log.Info("GameObjectPoolService deinitialize");
        }

        public async Task<PoolableGameObject> Instantiate(GameObject @object, float lifeTime = 3)
        {
            var poolableGameObject = _pollableObjects.FirstOrDefault(e => e.Status == PoolableGameObjectStatus.Free);
            if (poolableGameObject == null)
            {
                var instance = await UnityAsync.Instantiate(@object);
                poolableGameObject = new PoolableGameObject(lifeTime, instance);
            }
            poolableGameObject.Use();
            return poolableGameObject;
        }

        public void Destroy(PoolableGameObject @object)
        {
            @object.Release();
            UnityAsync.Destroy(@object.Instance);
            _pollableObjects.Remove(@object);
        }
        
        public void Clear()
        {
            foreach (var pollableObject in _pollableObjects)
            {
                pollableObject.Release();
                UnityAsync.Destroy(pollableObject.Instance);
            }
            _pollableObjects.Clear();
        }

        private void CheckObjects()
        {
            var dieObjects = _pollableObjects.Where(e => e.Status == PoolableGameObjectStatus.Dead);
            foreach (var dieObject in dieObjects)
            {
                UnityAsync.Destroy(dieObject.Instance);
                _pollableObjects.Remove(dieObject);
            }

            var freeObjects = _pollableObjects.Where(e => e.Status == PoolableGameObjectStatus.Free);
            foreach (var freeObject in freeObjects)
            {
                freeObject.Instance.transform.SetParent(Transfrom,false);
            }

            _timer.Start(CheckObjects);
        }


    }
}

