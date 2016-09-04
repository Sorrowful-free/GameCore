using System.Collections;
using UnityEngine;

namespace GameCore.Core.UnityThreading
{
    public static class UnityMainThreadExtention
    {
        public static Coroutine StartAsCoroutine(this IEnumerator enumerator)
        {
            if (enumerator != null)
            {
                return UnityMainThread.StartGlobalCoroutine(enumerator);
            }
            return null;
        }

        public static void StopCoroutine(this IEnumerator enumerator)
        {
            if (enumerator != null)
            {
                UnityMainThread.StopGlobalCoroutine(enumerator);
            }
        }

        public static void StopCoroutine(this Coroutine coroutine)
        {
            if (coroutine != null)
            {
                UnityMainThread.StopGlobalCoroutine(coroutine);
            }
        }
    }
}