using System;
using System.Threading.Tasks;
using UnityEngine;

namespace GameCore.Core.Base.Attributes
{
    public class ResourceGameObjectLoadAttribute :Attribute,IGameObjectLoadAttribute
    {
        public Task<GameObject> LoadGameObject()
        {
            throw new NotImplementedException();
        }

        public Task UnloadGameObject()
        {
            throw new NotImplementedException();
        }
    }
}
