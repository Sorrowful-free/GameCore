using System;
using System.Collections;
using System.Runtime.CompilerServices;
using GameCore.Core.UnityThreading;
using UnityEngine;

namespace GameCore.Core.Base.Async
{
    public class WaitWhileOperation : IAwaitable
    {
        private readonly Func<bool> _waitWhile;

        public WaitWhileOperation(Func<bool> waitWhile)
        {
            _waitWhile = waitWhile;
        }

        private IEnumerator AsyncWaitWhile(Action callback)
        {
            yield return new WaitWhile(_waitWhile);
        }
        public IAwaiter GetAwaiter()
        {
            return new CallbackAwaiter((c)=>AsyncWaitWhile(c).StartAsCoroutine());       
        }
    }
}
