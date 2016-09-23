using System.Threading.Tasks;
using UnityEngine;

namespace GameCore.Core.Services.UI.Attributes
{
    
    public interface IUIViewAttribute
    {
        Task<GameObject> LoadViewGameObject();

        Task UnloadViewGameObject();
    }
}
