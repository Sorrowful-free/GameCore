using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using GameCore.Core.Base;
using JetBrains.Annotations;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GameCore.Core.UnityThreading
{
    public class UnityMainThread : BaseMonoBehaviour
    {
        public const int MAX_TIME_FOR_EXECUTE_TIME = 50;
        public static Thread MainThread { get; private set; }
        private static Queue<Action> _queue = new Queue<Action>();
        private static UnityMainThread _instance;

        public static void QueueUserWorkItem(Action action)
        {
            _queue.Enqueue(action);
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
            UnitySynchronizationContext.MakeUnity();
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
                var action = _queue.Dequeue();
                try
                {
                    var handler = action;
                    if (handler != null)
                    {
                        action();
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    throw e;
                }
                
            }
            _stopwatch.Stop();
        }
    }
}
