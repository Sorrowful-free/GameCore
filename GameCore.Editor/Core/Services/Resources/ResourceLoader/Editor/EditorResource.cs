using System.Threading.Tasks;
using GameCore.Core.Services.Resources.ResourceLoader;
using UnityEditor.VersionControl;
using UnityEngine;

namespace GameCore.Editor.Core.Services.Resources.ResourceLoader.Editor
{
    public class EditorResource : IResource
    {
        public EditorResource(Object asset)
        {
            Asset = asset;
        }
        public int ReferenceCount { get; set; }
        public bool IsLoaded => true;
        public Object Asset { get; }
        public async Task<Object> WaitLoading()
        {
            return Asset;
        }
    }
}
