using System;
using System.Collections;
using System.Runtime.CompilerServices;
using GameCore.Core.Base.Async;
using GameCore.Core.Extentions;
using GameCore.Core.UnityThreading;
using UnityEngine;

namespace GameCore.Core.Base
{
    public struct GameTimer :IAwaitable
    {
        private Coroutine _coroutine;
        private float _deltaTime;
        private bool _unscaledTime;
        private bool _isPaused;
        public bool IsPaused
        {
            get { return _isPaused; }
        }

        public bool IsDone { get { return _deltaTime <= 0; } }

        public float DeltaTime
        {
            get { return _deltaTime; }
        }

        public bool UnscaledTime
        {
            get { return _unscaledTime; }
        }

        public GameTimer(float deltaTime,bool unscaledTime = false)
        {
            _coroutine = null;
            _deltaTime = deltaTime;
            _unscaledTime = unscaledTime;
            _isPaused = false;
        }

        public void Start(Action onDone)
        {
            _coroutine = AsyncStart(onDone).StartAsCoroutine();
        }

        public void Stop()
        {
            _coroutine.StopCoroutine();
            _coroutine = null;
        }

        public void Pause()
        {
            _isPaused = true;
        }

        public void Resume()
        {
            _isPaused = false;
        }

        private IEnumerator AsyncStart(Action onDone)
        {
            while (_deltaTime>0)
            {
                if(_isPaused)
                _deltaTime -= (_unscaledTime ? Time.unscaledTime : Time.deltaTime);
                yield return 0;
            }
            onDone.SafeInvoke();
        }

        public IAwaiter GetAwaiter()
        {
            return new CallbackAwaiter(Start);
        }
    }
}
