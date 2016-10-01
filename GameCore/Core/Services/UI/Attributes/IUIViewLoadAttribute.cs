using System.Threading.Tasks;
using UnityEngine;

namespace GameCore.Core.Services.UI.Attributes
{
    
    public interface IUIViewLoadAttribute
    {
        Task<GameObject> LoadViewGameObject();

        Task UnloadViewGameObject();
    }
}
