using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Runtime.Async.UnityTasks
{
    internal class UnityMainThreadExecutionTaskQueue : MonoBehaviour
    {
        private static UnityMainThreadExecutionTaskQueue _instance;

        private readonly float _maxExecutionTime = 1.0f / Application.targetFrameRate / 2; // only half frame time

        internal static Queue<Task> UpdateTask { get; } = new Queue<Task>();

        internal static Queue<Task> LateUpdateTask { get; } = new Queue<Task>();

        internal static Queue<Task> FixedUpdateTask { get; } = new Queue<Task>();

        [RuntimeInitializeOnLoadMethod]
        private static void InitializeExecutionQueue()
        {
            if (_instance == null)
            {
                var gameObject = new GameObject(nameof(UnityMainThreadExecutionTaskQueue));
                _instance = gameObject.AddComponent<UnityMainThreadExecutionTaskQueue>();
            }
        }

        private void Update()
        {
            var startTime = Time.unscaledTime;
            while (Time.unscaledTime - startTime < _maxExecutionTime)
            {
                var task = UpdateTask.Dequeue();
                if (!task.IsCanceled)
                    task.Start();
            }
        }

        private void LateUpdate()
        {
            var startTime = Time.unscaledTime;
            while (Time.unscaledTime - startTime < _maxExecutionTime)
            {
                var task = LateUpdateTask.Dequeue();
                if (!task.IsCanceled)
                    task.Start();
            }
        }

        private void FixedUpdate()
        {
            var startTime = Time.fixedUnscaledTime;
            while (Time.fixedUnscaledTime - startTime < _maxExecutionTime)
            {
                var task = FixedUpdateTask.Dequeue();
                if (!task.IsCanceled)
                    task.Start();
            }
        }
    }
}