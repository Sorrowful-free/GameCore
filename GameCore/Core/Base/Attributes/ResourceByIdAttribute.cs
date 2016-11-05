using System;
using System.Threading.Tasks;
using GameCore.Core.Application;
using GameCore.Core.Services.Resources;
using UnityEngine;

namespace GameCore.Core.Base.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ResourceByIdAttribute :Attribute,IGameObjectLoadAttribute
    {
        private readonly int _id;

        public ResourceByIdAttribute(int id)
        {
            _id = id;
        }

        public async Task<GameObject> LoadGameObject()
        {
            var resourceService = await GameApplication.GetService<ResourceService>();
            return await resourceService.GetAsset<GameObject>(_id).Load();
        }

        public async Task UnloadGameObject()
        {
            var resourceService = await GameApplication.GetService<ResourceService>();
            await resourceService.GetAsset<GameObject>(_id).Unload();
        }
    }
}
