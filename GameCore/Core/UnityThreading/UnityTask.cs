using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace GameCore.Core.UnityThreading
{
    public static class UnityTask
    {
        private static TaskFactory _mainThreadFactory;
        public static TaskFactory MainThreadFactory
        {
            get
            {
                if (_mainThreadFactory == null)
                {
                    _mainThreadFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None,
                        TaskContinuationOptions.DenyChildAttach, UnityMainThreadTaskScheduler.Instance);
                }
                return _mainThreadFactory;
            }
        }

        private static TaskFactory _threadPoolFactory;
        public static TaskFactory ThreadPoolFactory
        {
            get
            {
                if (_threadPoolFactory == null)
                {
                    if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer)
                        _threadPoolFactory = MainThreadFactory;
                    else
                    {
                        _threadPoolFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None,
                            TaskContinuationOptions.DenyChildAttach, UnityThreadPoolScheduler.Instance);
                    }
                        
                }
                return _mainThreadFactory;
            }
        }
    }

    public class UnityTask<TResult>
    {
        private static TaskFactory<TResult> _mainThreadFactory;
        public static TaskFactory<TResult> MainThreadFactory
        {
            get
            {
                if (_mainThreadFactory == null)
                {
                    _mainThreadFactory = new TaskFactory<TResult>(CancellationToken.None, TaskCreationOptions.None,
                        TaskContinuationOptions.DenyChildAttach, UnityMainThreadTaskScheduler.Instance);
                }
                return _mainThreadFactory;
            }
        }

        private static TaskFactory<TResult> _threadPoolFactory;
        public static TaskFactory<TResult> ThreadPoolFactory
        {
            get
            {
                if (_threadPoolFactory == null)
                {
                    if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer)
                        _threadPoolFactory = MainThreadFactory;
                    else
                    {
                        _threadPoolFactory = new TaskFactory<TResult>(CancellationToken.None, TaskCreationOptions.None,
                            TaskContinuationOptions.DenyChildAttach, UnityThreadPoolScheduler.Instance);
                    }

                }
                return _mainThreadFactory;
            }
        }
    }
}
