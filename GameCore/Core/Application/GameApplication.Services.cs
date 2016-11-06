using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCore.Core.Application.Interfaces.Services;
using GameCore.Core.Base;
using GameCore.Core.Base.Async;
using GameCore.Core.Base.Attributes;
using GameCore.Core.Base.Dependency;
using GameCore.Core.Extentions;
using GameCore.Core.Logging;
using GameCore.Core.UnityThreading;
using UnityEngine;

namespace GameCore.Core.Application
{
    public partial class GameApplication
    {
        private readonly static List<IPauseService> _pauseServices = new List<IPauseService>();
        private readonly static List<IUpdateService> _updateServices = new List<IUpdateService>();
        private readonly static List<IFixedUpdateServices> _fixedUpdateServices = new List<IFixedUpdateServices>();
        private readonly static List<ILateUpdateService> _lateUpdateServices = new List<ILateUpdateService>();
        private readonly static Dictionary<Type,IService> _services = new Dictionary<Type, IService>();

        private static void PauseServices(bool isPause)
        {
            foreach (var service in _pauseServices)
            {
                service.ServicePause(isPause);
            }
        }

        private static void UpdateServices()
        {
            foreach (var service in _updateServices)
            {
                service.ServiceUpdate();
            }
        }

        private static void FixedUpdateServices()
        {
            foreach (var service in _fixedUpdateServices)
            {
                service.ServiceFixedUpdate();
            }
        }

        private static void LateUpdateServices()
        {
            foreach (var service in _lateUpdateServices)
            {
                service.ServiceLateUpdate();
            }
        }

        public static async Task<TService> GetService<TService>() where TService : IService
        {
            return (TService)await GetService(typeof(TService));
        }

        public static async Task<IService> GetService(Type serviceType)
        {
            var service = default(IService);
            if (!_services.TryGetValue(serviceType, out service))
            {
                service = await CreateServiceInstance(serviceType);
                if (service is IPauseService)
                {
                    _pauseServices.Add((IPauseService)service);
                }
                else if (service is IUpdateService)
                {
                    _updateServices.Add((IUpdateService)service);
                }
                else if (service is IFixedUpdateServices)
                {
                    _fixedUpdateServices.Add((IFixedUpdateServices)service);
                }
                else if (service is ILateUpdateService)
                {
                    _lateUpdateServices.Add((ILateUpdateService)service);
                }
                _services.Add(serviceType,service);
            }
            return service;
        }

        public static Task DestroyService<TService>() where TService : IService
        {
            return DestroyService(typeof (TService));
        }

        public static async Task DestroyService(Type serviceType)
        {

            var service = default(IService);
            if (_services.TryGetValue(serviceType, out service))
            {
                if (service is IPauseService)
                {
                    _pauseServices.Remove((IPauseService)service);
                }
                else if (service is IUpdateService)
                {
                    _updateServices.Remove((IUpdateService)service);
                }
                else if (service is IFixedUpdateServices)
                {
                    _fixedUpdateServices.Remove((IFixedUpdateServices)service);
                }
                else if (service is ILateUpdateService)
                {
                    _lateUpdateServices.Remove((ILateUpdateService)service);
                }
                _services.Remove(serviceType);
                await DestroyServiceInstance(service);
            }
        }

        private static async Task<IService> CreateServiceInstance(Type serviceType)
        {
            //check for dependency
            if (serviceType.IsInterface)
            {
                serviceType =
                    await
                        UnityTask<Type>.ThreadPoolFactory.StartNew(
                            () => DependencyInjector.GetDependencyType(serviceType));
            }
            
            var service = default(IService);
            //check is unity component or not
            if (serviceType.IsSubclassOf(typeof (Component))) // unity huyunuty
            {
                var attribute = serviceType.GetAttributeByInterface<IGameObjectLoadAttribute>();
                var serviceGameObject = default(GameObject);
                if (attribute != null)
                {
                    serviceGameObject = await UnityAsync.Instantiate(await attribute.LoadGameObject());
                    attribute.UnloadGameObject();
                }
                else
                {
                    serviceGameObject =
                        await
                            UnityTask<GameObject>.MainThreadFactory.StartNew(
                                () => new GameObject(serviceType.Name, serviceType));

                }
                service = await UnityTask<IService>.MainThreadFactory.StartNew(() =>
                {
                    serviceGameObject.transform.SetParent(_instance.transform, false);
                    return (IService)serviceGameObject.GetComponent(serviceType);
                });
            }
            else
            {
                service = await UnityTask<IService>.ThreadPoolFactory.StartNew(() => (IService)Activator.CreateInstance(serviceType));
            }
            await service.Initialize();
            Log.Info("[GameApplication]: {0} initialize", serviceType.Name);
            return service;
        }

        private static async Task DestroyServiceInstance(IService service)
        {
            var serviceType = service.GetType();
            await service.Deinitialize();
            if (serviceType.IsSubclassOf(typeof (Component)))
            {
                var component = (Component) service;
                await UnityAsync.Destroy(component.gameObject);
            }
            Log.Info("[GameApplication]: {0} deinitialized", serviceType.Name);
        }
    }
}
