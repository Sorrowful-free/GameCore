using System.Threading.Tasks;
using UnityEngine;

namespace GameCore.Core.Base.Attributes
{
    
    public interface IGameObjectLoadAttribute
    {
        Task<GameObject> LoadGameObject();

        Task UnloadGameObject();
    }
}
