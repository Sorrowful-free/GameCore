using System;

namespace GameCore.Core.Extentions
{
    public static class ActionExtention
    {
        public static void SafeInvoke(this Action action)
        {
            var handler = action;
            if (handler != null)
            {
                handler();
            }
        }

        public static void SafeInvoke<T0>(this Action<T0> action,T0 var0)
        {
            var handler = action;
            if (handler != null)
            {
                handler(var0);
            }
        }

        public static void SafeInvoke<T0,T1>(this Action<T0,T1> action, T0 var0,T1 var1)
        {
            var handler = action;
            if (handler != null)
            {
                handler(var0,var1);
            }
        }

        public static void SafeInvoke<T0, T1, T2>(this Action<T0, T1, T2> action, T0 var0, T1 var1, T2 var2)
        {
            var handler = action;
            if (handler != null)
            {
                handler(var0, var1, var2);
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
