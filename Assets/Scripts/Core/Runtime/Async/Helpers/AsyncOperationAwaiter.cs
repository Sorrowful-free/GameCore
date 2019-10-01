using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Core.Runtime.Async.Helpers
{
    public class AsyncOperationAwaiter<TAsyncOperation> : INotifyCompletion where TAsyncOperation : AsyncOperation
    {
        private readonly TAsyncOperation _asyncOperation;
        private Action _continuationCallback;

        public AsyncOperationAwaiter(TAsyncOperation asyncOperation)
        {
            _asyncOperation = asyncOperation;
        }

        public bool IsCompleted => _asyncOperation.isDone;

        public void OnCompleted(Action continuation)
        {
            _asyncOperation.completed += OnAsyncOperationCompleted;
            _continuationCallback = continuation;
        }

        private void OnAsyncOperationCompleted(AsyncOperation asyncOperation)
        {
            _asyncOperation.completed -= OnAsyncOperationCompleted;
            _continuationCallback?.Invoke();
        }

        public TAsyncOperation GetResult()
        {
            return _asyncOperation;
        }
    }
}