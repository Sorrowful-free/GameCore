using System.Threading.Tasks;
using GameCore.Core.UnityThreading;
using UnityEngine;

namespace GameCore.Core.Base
{
    public static class UnityAsync 
    {
        public async static Task<TObject> Instantiate<TObject>(TObject original) where TObject : Object
        {
            var task = new Task<TObject>(() => Object.Instantiate(original));
            task.Start(UnityTaskScheduler.Instance);
            return await task;
        }

        public async static Task Destroy(Object obj)
        {
            var task = new Task(() =>
            {
                if (UnityEngine.Application.isEditor)
                    Object.DestroyImmediate(obj);
                else
                    Object.Destroy(obj);
            }
            
            );
            task.Start(UnityTaskScheduler.Instance);
            await task;
        }
    }
}
