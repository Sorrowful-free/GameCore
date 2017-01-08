using System;
using System.Collections;
using System.Runtime.CompilerServices;
using GameCore.Core.Base.Async;
using GameCore.Core.Extentions;
using GameCore.Core.UnityThreading;
using UnityEngine;

namespace GameCore.Core.Base
{
    public struct GameTimer
    {
        private Coroutine _coroutine;
        private float _currentTime;
        private bool _unscaledTime;
        private bool _isPaused;
        private float _deltaTime;
        public bool IsPaused
        {
            get { return _isPaused; }
        }

        public bool IsDone { get { return _currentTime <= 0; } }

        public float CurrentTime
        {
            get { return _currentTime; }
        }

        public bool UnscaledTime
        {
            get { return _unscaledTime; }
        }

        public GameTimer(float deltaTime, bool unscaledTime = false)
        {
            _coroutine = null;
            _deltaTime = _currentTime = deltaTime;
            _unscaledTime = unscaledTime;
            _isPaused = false;
        }

        public void Start(Action onDone)
        {
            _currentTime = _deltaTime;//перезапуск таймера
            _coroutine = AsyncStart(onDone).StartAsCoroutine();
        }

        public AwaitableOperation Start()
        {
            return new AwaitableOperation(Start);
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
            while (_currentTime>0)
            {
                if (!_isPaused)
                _currentTime -= (_unscaledTime ? Time.unscaledTime : Time.deltaTime);
                yield return 0;
            }
            onDone.SafeInvoke();
        }
    }
}
