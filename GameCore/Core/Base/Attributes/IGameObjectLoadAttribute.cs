using System.Threading.Tasks;
using UnityEngine;

namespace GameCore.Core.Services.UI.Attributes
{
    
    public interface IGameObjectLoadAttribute
    {
        Task<GameObject> LoadViewGameObject();

        Task UnloadViewGameObject();
    }
}
