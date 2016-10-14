using System;
using System.Runtime.CompilerServices;
using GameCore.Core.Extentions;

namespace GameCore.Core.Base.Async
{
    public class CallbackAwaiter : IAwaiter
    {
        private Action<Action> _method;
        private bool _isCompleted;
        public CallbackAwaiter(Action<Action> method)
        {
            _method = method;
        }

        public bool IsCompleted
        {
            get { return _isCompleted; }
        }

        public void GetResult()
        {
        }

        public void OnCompleted(Action continuation)
        {
            _method.SafeInvoke(() =>
            {
                _isCompleted = true;
                continuation.SafeInvoke();
            });
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            _method.SafeInvoke(() =>
            {
                _isCompleted = true;
                continuation.SafeInvoke();
            });
        }
    }

    public class CallbackAwaiter<TResult> : IAwaiter<TResult>
    {
        private Action<Action<TResult>> _method;
        private TResult _result;
        private bool _isCompleted;
        public CallbackAwaiter(Action<Action<TResult>> method)
        {
            _method = method;
        }

        public bool IsCompleted
        {
            get { return _isCompleted; }
        }

        public TResult GetResult()
        {
            return _result;
        }

        public void OnCompleted(Action continuation)
        {
            _method.SafeInvoke((result) =>
            {
                _result = result;
                _isCompleted = true;
                continuation.SafeInvoke();
            });
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            _method.SafeInvoke((result) =>
            {
                _result = result;
                _isCompleted = true;
                continuation.SafeInvoke();
            });
        }
    }
}
