using System;
using GameCore.Core.Base;
using UnityEngine;

namespace GameCore.Core.Services.GameObjectPool
{
    public class PoolableGameObject
    {
        private readonly GameObject _instance;
        private readonly Transform _container;
        
        private readonly GameTimer _lifeTime;
        public PoolableGameObjectStatus Status { get; private set; }

        public GameObject Instance
        {
            get { return _instance; }
        }

        public PoolableGameObject(float lifeTime,GameObject instance)
        {
            _instance = instance;
            _lifeTime = new GameTimer(lifeTime);
        }

        public void Use()
        {
            if(Status == PoolableGameObjectStatus.Dead)
                throw new AggregateException("poll object is dead");
            Status = PoolableGameObjectStatus.Busy;
            _lifeTime.Stop();
            _instance.transform.SetParent(null, false);
            _instance.SetActive(true);
        }

        public void Release()
        {
            if (Status == PoolableGameObjectStatus.Dead)
                throw new AggregateException("poll object is dead");
            _lifeTime.Stop();
            _lifeTime.Start(OnDie);
            _instance.SetActive(false);
            Status = PoolableGameObjectStatus.Free;
        }

        private void OnDie()
        {
            Status = PoolableGameObjectStatus.Dead;
        }

        public static implicit operator GameObject(PoolableGameObject poolableGameObject)
        {
            return poolableGameObject._instance;
        }
        
    }
}