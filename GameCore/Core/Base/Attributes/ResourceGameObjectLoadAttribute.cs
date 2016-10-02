using System;
using System.Threading.Tasks;
using GameCore.Core.Services.UI.Attributes;
using UnityEngine;

namespace GameCore.Core.Attributes
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
