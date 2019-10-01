using System.Threading.Tasks;
using UnityEngine;

namespace Core.Runtime.Async.Helpers
{
    public class AsyncOperationHandler<TAsyncOperation>
        where TAsyncOperation : AsyncOperation
    {
        private readonly TAsyncOperation _asyncOperation;
        private readonly TaskCompletionSource<TAsyncOperation> _completionSource;

        public AsyncOperationHandler(TAsyncOperation asyncOperation)
        {
            _asyncOperation = asyncOperation;
            _completionSource = new TaskCompletionSource<TAsyncOperation>();
            _asyncOperation.completed += OnAsyncOperationComplete;
        }

        public Task<TAsyncOperation> Task => _completionSource.Task;

        private void OnAsyncOperationComplete(AsyncOperation asyncOperation)
        {
            _asyncOperation.completed -= OnAsyncOperationComplete;
            _completionSource.SetResult(_asyncOperation);
        }
    }
}