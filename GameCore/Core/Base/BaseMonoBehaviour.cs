using UnityEngine;

namespace GameCore.Core.Base
{
    public class BaseMonoBehaviour: MonoBehaviour
    {
        private Transform _transform;
        public Transform Transfrom
        {
            get
            {
                if (_transform == null)
                {
                    _transform = transform;
                }
                return _transform;
            }
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
        }

        protected virtual void Update()
        {
        }

        protected virtual void FixedUpdate()
        {
        }

        protected virtual void LateUpdate()
        {
        }

        protected virtual void OnDestroy()
        {
        }

        protected virtual void OnDisable()
        {
        }
    }
}
