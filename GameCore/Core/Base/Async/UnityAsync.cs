using System.Threading.Tasks;
using GameCore.Core.UnityThreading;
using UnityEngine;

namespace GameCore.Core.Base.Async
{
    public static class UnityAsync 
    {
        public async static Task<TObject> Instantiate<TObject>(TObject original) where TObject : Object
        {
            var task = UnityTask<TObject>.MainThreadFactory.StartNew(() => Object.Instantiate(original));
            return await task;
        }

        public async static Task Destroy(Object obj)
        {
            var task = UnityTask.MainThreadFactory.StartNew(() =>
            {
                if (UnityEngine.Application.isEditor)
                    Object.DestroyImmediate(obj);
                else
                    Object.Destroy(obj);
            });
            await task;
        }
    }
}
