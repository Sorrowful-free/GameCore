using System.Threading.Tasks;
using UnityEngine;

namespace GameCore.Core.Services.Resources.ResourceLoader
{
    public interface IResource 
    {
        int ReferenceCount { get; set; }
        bool IsLoaded { get; }
        Object Asset { get; }
        Task<Object> WaitLoading();
    }
}
