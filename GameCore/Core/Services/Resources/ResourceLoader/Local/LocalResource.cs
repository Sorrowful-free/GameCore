using System.Threading.Tasks;
using GameCore.Core.Extentions;
using UnityEngine;

namespace GameCore.Core.Services.Resources.ResourceLoader.Local
{
    public class LocalResource : IResource
    {
        private readonly ResourceRequest _resourceRequest;
        public int ReferenceCount { get; set; }
        public bool IsLoaded => _resourceRequest.isDone;
        public Object Asset =>  _resourceRequest.asset;
        public LocalResource(ResourceRequest resourceRequest)
        {
            _resourceRequest = resourceRequest;
        }
        public async Task<Object> WaitLoading()
        {
            return (await _resourceRequest.ToAwaitable()).asset;
        }
    }
}