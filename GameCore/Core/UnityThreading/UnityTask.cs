using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace GameCore.Core.UnityThreading
{
    public class UnityTask
    {
        private static TaskFactory _factory;
        public static TaskFactory Factory
        {
            get
            {
                if (_factory == null)
                {
                    if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer)
                        _factory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None,
                            TaskContinuationOptions.DenyChildAttach, UnityTaskScheduler.Instance);
                    else
                        _factory = Task.Factory;
                }
                return _factory;
            }
            
        }
    }

    public class UnityTask<TResult>
    {
        private static TaskFactory<TResult> _factory;
        public static TaskFactory<TResult> Factory
        {
            get
            {
                if (_factory == null)
                {
                    if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer)
                        _factory = new TaskFactory<TResult>(CancellationToken.None, TaskCreationOptions.None,
                            TaskContinuationOptions.DenyChildAttach, UnityTaskScheduler.Instance);
                    else
                        _factory = Task<TResult>.Factory;

                }
                return _factory;
            }

        }
    }
}
