using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCore.Core.Base.Async;
using GameCore.Core.UnityThreading;
using UnityEngine;

namespace GameCore.Core.Extentions
{
    public static class UnityAsyncExtentions
    {
        public static AwaitableOperation<TOperation> ToAwaitable<TOperation>(this TOperation operation) where TOperation : AsyncOperation
        {
            return new AwaitableOperation<TOperation>((onDone)=> { AsyncOperationCoroutine(operation, onDone).StartAsCoroutine(); });
        }

        private static IEnumerator AsyncOperationCoroutine<TOperation>(TOperation operation, Action<TOperation> onDone) where TOperation : AsyncOperation
        {
            yield return operation;
            onDone.SafeInvoke(operation);
        }
    }
}
