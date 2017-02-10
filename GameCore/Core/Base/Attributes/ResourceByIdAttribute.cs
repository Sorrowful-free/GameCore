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
            var resourceService = await GameApplication.InitializeService<ResourceService>();
            return await resourceService.LoadAsset<GameObject>(_id);
        }

        public async Task UnloadGameObject()
        {
            var resourceService = await GameApplication.InitializeService<ResourceService>();
            await resourceService.UnloadAsset(_id);
        }
    }
}
