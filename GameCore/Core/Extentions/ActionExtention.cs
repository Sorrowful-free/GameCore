using System;
using GameCore.Core.UnityThreading;

namespace GameCore.Core.Extentions
{
    public static class ActionExtention
    {
        private static void Invoke(Action action,
            ActionInvokationType invokationType = ActionInvokationType.CurrentThread)
        {
            switch (invokationType)
            {
                default:
                case ActionInvokationType.CurrentThread:
                    action();
                    break;
                case ActionInvokationType.MainThread:
                    UnityMainThread.QueueUserWorkItem((s) => action(), null);
                    break;
                case ActionInvokationType.ThreadPool:
                    UnityThreadPool.QueueUserWorkItem((s) => action(), null);
                    break;
            }
        }

        public static void SafeInvoke(this Action action, ActionInvokationType invokationType = ActionInvokationType.CurrentThread)
        {
            var handler = action;
            if (handler != null)
            {
                Invoke(action,invokationType);
            }
        }

        public static void SafeInvoke<T0>(this Action<T0> action,T0 var0, ActionInvokationType invokationType = ActionInvokationType.CurrentThread)
        {
            var handler = action;
            if (handler != null)
            {
                Invoke(()=> handler(var0), invokationType);
            }
        }

        public static void SafeInvoke<T0,T1>(this Action<T0,T1> action, T0 var0,T1 var1, ActionInvokationType invokationType = ActionInvokationType.CurrentThread)
        {
            var handler = action;
            if (handler != null)
            {
                Invoke(() => handler(var0, var1), invokationType);
            }
        }

        public static void SafeInvoke<T0, T1, T2>(this Action<T0, T1, T2> action, T0 var0, T1 var1, T2 var2, ActionInvokationType invokationType = ActionInvokationType.CurrentThread)
        {
            var handler = action;
            if (handler != null)
            {
                Invoke(() => handler(var0, var1, var2), invokationType);
            }
        }

        public static void ClearAllHandlers(this Action action)
        {
            if (action == null)
            {
                return;
            }

            foreach (Delegate d in action.GetInvocationList())
            {
                action -= (Action)d;
            }
        }

        public static void ClearAllHandlers<T0>(this Action<T0> action)
        {
            if (action == null)
            {
                return;
            }

            foreach (Delegate d in action.GetInvocationList())
            {
                action -= (Action<T0>)d;
            }
        }

        public static void ClearAllHandlers<T0,T1>(this Action<T0,T1> action)
        {
            if (action == null)
            {
                return;
            }

            foreach (Delegate d in action.GetInvocationList())
            {
                action -= (Action<T0,T1>)d;
            }
        }

        public static void ClearAllHandlers<T0, T1, T2>(this Action<T0, T1, T2> action)
        {
            if (action == null)
            {
                return;
            }

            foreach (Delegate d in action.GetInvocationList())
            {
                action -= (Action<T0, T1, T2>)d;
            }
        }
    }
}
