using System.Threading.Tasks;
using Core.Runtime.Async.Helpers;
using UnityEngine;

namespace Core.Runtime.Async
{
    public static class AsyncOperationExtensions
    {
        public static Task<TAsyncOperation> ToTask<TAsyncOperation>(this TAsyncOperation asyncOperation)
            where TAsyncOperation : AsyncOperation
        {
            var handler = new AsyncOperationHandler<TAsyncOperation>(asyncOperation);
            return handler.Task;
        }

        public static AsyncOperationAwaiter<TAsyncOperation> GetAwaiter<TAsyncOperation>(
            this TAsyncOperation asyncOperation)
            where TAsyncOperation : AsyncOperation
        {
            return new AsyncOperationAwaiter<TAsyncOperation>(asyncOperation);
        }
    }
}