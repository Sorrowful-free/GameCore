using System.Threading.Tasks;
using GameCore.Core.Extentions;
using UnityEngine;

namespace GameCore.Core.Services.Resources.ResourceLoader.Bundle
{
    public class BundleResource : IResource
    {
        private readonly AssetBundleRequest _assetBundleRequest;
        public int ReferenceCount { get; set; }
        public bool IsLoaded => _assetBundleRequest.isDone;
        public Object Asset => _assetBundleRequest.asset;
        public BundleResource(AssetBundleRequest assetBundleRequest)
        {
            _assetBundleRequest = assetBundleRequest;
        }
        public async Task<Object> WaitLoading()
        {
            return (await _assetBundleRequest.ToAwaitable()).asset;
        }
    }
}
