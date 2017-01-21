﻿using System;
using GameCore.Core.UnityThreading;

namespace GameCore.Core.Extentions
{
    public static class FuncExtention
    {
        public static TResult SafeInvoke<TResult>(this Func<TResult> func)
        {
            var handler = func;
            if (handler != null)
            {
              return handler();
            }
            return default(TResult);
        }

        public static TResult SafeInvoke<T0,TResult>(this Func<T0,TResult> action, T0 var0)
        {
            var handler = action;
            if (handler != null)
            {
               return handler(var0);
            }
            return default(TResult);
        }

        public static TResult SafeInvoke<T0, T1, TResult>(this Func<T0, T1,TResult> action, T0 var0, T1 var1)
        {
            var handler = action;
            if (handler != null)
            {
                return handler(var0, var1);
            }
            return default(TResult);
        }

        public static TResult SafeInvoke<T0, T1, T2, TResult>(this Func<T0, T1, T2, TResult> action, T0 var0, T1 var1, T2 var2)
        {
            var handler = action;
            if (handler != null)
            {
                return handler(var0, var1, var2);
            }
            return default(TResult);
        }

        public static void ClearAllHandlers<TResult>(this Func<TResult> func)
        {
            if (func == null)
            {
                return;
            }

            foreach (Delegate d in func.GetInvocationList())
            {
                func -= (Func<TResult>)d;
            }
        }

        public static void ClearAllHandlers<T0, TResult>(this Func<T0, TResult> func)
        {
            if (func == null)
            {
                return;
            }

            foreach (Delegate d in func.GetInvocationList())
            {
                func -= (Func<T0, TResult>)d;
            }
        }

        public static void ClearAllHandlers<T0, T1, TResult>(this Func<T0, T1, TResult> func)
        {
            if (func == null)
            {
                return;
            }

            foreach (Delegate d in func.GetInvocationList())
            {
                func -= (Func<T0, T1, TResult>)d;
            }
        }

        public static void ClearAllHandlers<T0, T1, T2, TResult>(this Func<T0, T1, T2, TResult> func)
        {
            if (func == null)
            {
                return;
            }

            foreach (Delegate d in func.GetInvocationList())
            {
                func -= (Func<T0, T1, T2, TResult>)d;
            }
        }
    }
}
