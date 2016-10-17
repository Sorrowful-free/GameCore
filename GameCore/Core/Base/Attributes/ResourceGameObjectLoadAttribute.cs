using System;
using System.Threading.Tasks;
using UnityEngine;

namespace GameCore.Core.Base.Attributes
{
    public class ResourceGameObjectLoadAttribute :Attribute,IGameObjectLoadAttribute
    {
        public Task<GameObject> LoadViewGameObject()
        {
            throw new NotImplementedException();
        }

        public Task UnloadViewGameObject()
        {
            throw new NotImplementedException();
        }
    }
}
