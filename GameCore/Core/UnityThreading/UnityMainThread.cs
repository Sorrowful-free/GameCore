using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using GameCore.Core.Base;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GameCore.Core.UnityThreading
{
    public class UnityMainThread : BaseMonoBehaviour
    {
        public const int MAX_TIME_FOR_EXECUTE_TIME = 50;
        public static Thread MainThread { get; private set; }
        private static ConcurrentQueue<Tuple<WaitCallback,object>> _queue = new ConcurrentQueue<Tuple<WaitCallback, object>>();
        private static UnityMainThread _instance;

        public static void QueueUserWorkItem(WaitCallback callback,object state)
        {
            _queue.Enqueue(new Tuple<WaitCallback, object>(callback, state));
        }

        public static Coroutine StartGlobalCoroutine(IEnumerator coroutine)
        {
            return _instance.StartCoroutine(coroutine);
        }

        public static void StopGlobalCoroutine(Coroutine coroutine)
        {
            _instance.StopCoroutine(coroutine);
        }

        public static void StopGlobalCoroutine(IEnumerator ienumerator)
        {
            _instance.StopCoroutine(ienumerator);
        }

        public static void StopAllGlobalCoroutines()
        {
            _instance.StopAllCoroutines();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            MainThread = Thread.CurrentThread;
            var unityMainThread = FindObjectOfType<UnityMainThread>();
            if (unityMainThread == null)
                unityMainThread = new GameObject("UnityMainThread").AddComponent<UnityMainThread>();
            unityMainThread.gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable;
            _instance = unityMainThread;
        }

        private Stopwatch _stopwatch = new Stopwatch();
        
        protected override void Update()
        {
            _stopwatch.Reset();
            _stopwatch.Start();
            while (_queue.Count > 0 && _stopwatch.ElapsedMilliseconds < MAX_TIME_FOR_EXECUTE_TIME)
            {
                var callbackPair = default(Tuple<WaitCallback, object>);
                if (_queue.TryDequeue(out callbackPair))
                {
                    if (callbackPair.Item1 != null)
                        callbackPair.Item1(callbackPair.Item2);
                }
            }
            _stopwatch.Stop();
        }
    }
}
