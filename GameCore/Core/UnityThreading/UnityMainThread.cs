using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using GameCore.Core.Base;
using UnityEngine;

namespace GameCore.Core.UnityThreading
{
    public class UnityMainThread : BaseMonoBehaviour
    {
        public const float MaxPercentOfFrameTime = 0.3f;
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

        public static void Initialize()
        {
            MainThread = Thread.CurrentThread;
            var unityMainThread = FindObjectOfType<UnityMainThread>();
            if (unityMainThread == null)
                unityMainThread = new GameObject("UnityMainThread").AddComponent<UnityMainThread>();
            unityMainThread.gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable;
            _instance = unityMainThread;
        }

        private Stopwatch _stopwatch = new Stopwatch();
        private int _maxTimeForQueue;
        protected override void OnEnable()
        {
            base.OnEnable();
            _maxTimeForQueue = (int)(1000/UnityEngine.Application.targetFrameRate * MaxPercentOfFrameTime);
        }
        
        protected override void Update()
        {
            _stopwatch.Reset();
            _stopwatch.Start();
            while (_queue.Count > 0 && _stopwatch.ElapsedMilliseconds< _maxTimeForQueue)
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
                    //TODO loging
                }
            }
            _stopwatch.Stop();
        }
    }
}
