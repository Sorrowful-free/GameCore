using System.Threading.Tasks;
using Core.Runtime.Async.UnityTasks;
using UnityEngine;

namespace Core.Runtime.Async
{
    public static class UnityAsync
    {
        public static Task<Object> Instantiate(Object original)
        {
            return UnityTask<Object>.UpdateTaskFactory.StartNew(() => Object.Instantiate(original));
        }

        public static Task<Object> Instantiate(Object original, Vector3 position, Quaternion rotation)
        {
            return UnityTask<Object>.UpdateTaskFactory.StartNew(() => Object.Instantiate(original, position, rotation));
        }

        public static Task<Object> Instantiate(Object original, Transform parent, bool worldPositionStays)
        {
            return UnityTask<Object>.UpdateTaskFactory.StartNew(() =>
                Object.Instantiate(original, parent, worldPositionStays: worldPositionStays));
        }

        public static Task<Object> Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent)
        {
            return UnityTask<Object>.UpdateTaskFactory.StartNew(() =>
                Object.Instantiate(original, position, rotation, parent));
        }

        public static Task<TObject> Instantiate<TObject>(TObject original) where TObject : Object
        {
            return UnityTask<TObject>.UpdateTaskFactory.StartNew(() => Object.Instantiate(original));
        }

        public static Task<TObject> Instantiate<TObject>(TObject original, Vector3 position, Quaternion rotation)
            where TObject : Object
        {
            return UnityTask<TObject>.UpdateTaskFactory.StartNew(() =>
                Object.Instantiate(original, position, rotation));
        }

        public static Task<TObject> Instantiate<TObject>(TObject original, Transform parent, bool worldPositionStays)
            where TObject : Object
        {
            return UnityTask<TObject>.UpdateTaskFactory.StartNew(() =>
                Object.Instantiate(original, parent, worldPositionStays));
        }

        public static Task<TObject> Instantiate<TObject>(TObject original, Vector3 position, Quaternion rotation,
            Transform parent) where TObject : Object
        {
            return UnityTask<TObject>.UpdateTaskFactory.StartNew(() =>
                Object.Instantiate(original, position, rotation, parent));
        }

        public static Task Destroy(Object instance)
        {
            return UnityTask.UpdateTaskFactory.StartNew(() => Object.Destroy(instance));
        }
    }
}