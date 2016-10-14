using System;
using System.Runtime.CompilerServices;

namespace GameCore.Core.Base.Async
{
    public class AwaitableOperation : IAwaitable
    {
        private readonly Action<Action> _method;

        public AwaitableOperation(Action<Action> method)
        {
            _method = method;
        }

        public IAwaiter GetAwaiter()
        {
            return new CallbackAwaiter(_method);
        }
    }

    public class AwaitableOperation<TResult> : IAwaitable<TResult>
    {
        private readonly Action<Action<TResult>> _method;

        public AwaitableOperation(Action<Action<TResult>> method)
        {
            _method = method;
        }

        public IAwaiter<TResult> GetAwaiter()
        {
            return new CallbackAwaiter<TResult>(_method);
        }
    }
}
