using System;
using System.Threading;
using UnityEngine;

namespace GameCore.Core.UnityThreading
{
    public class UnitySynchronizationContext : SynchronizationContext
    {
        
        static UnitySynchronizationContext()
        {
            Default = Current;
            Unity = new UnitySynchronizationContext();
        }

        public static readonly SynchronizationContext Unity;
        public static readonly SynchronizationContext Default;

        public static void MakeUnity()
        {
            SetSynchronizationContext(Unity);
        }

        public static void MakeDefault()
        {
            SetSynchronizationContext(Default);
        }
        
        public override void Post(SendOrPostCallback d, object state)
        {
            UnityMainThread.QueueUserWorkItem(() =>
            {
               Send(d,state);
            });
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            if (d != null)
                d(state);
            
        }
    }
}
