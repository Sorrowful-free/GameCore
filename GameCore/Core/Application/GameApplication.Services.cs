using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCore.Core.Application.Interfaces.Services;
using GameCore.Core.Base;
using GameCore.Core.Base.Async;
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

        public static async Task<TService> InitializeService<TService>() where TService : IService
        {
            return (TService)await InitializeService(typeof(TService));
        }

        public static async Task<IService> InitializeService(Type serviceType)
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
                
            }
            return service;
        }

        public static Task DeinitializeService<TService>() where TService : IService
        {
            return DeinitializeService(typeof (TService));
        }

        public static async Task DeinitializeService(Type serviceType)
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
                DestroyServiceInstance(service);
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
                service = await UnityTask<IService>.MainThreadFactory.StartNew(() =>
                {
                    var gameObject = new GameObject(serviceType.Name);
                    gameObject.transform.SetParent(_instance.transform,false);
                    return (IService)gameObject.AddComponent(serviceType);
                });
            }
            else
            {
                service = await UnityTask<IService>.ThreadPoolFactory.StartNew(() => (IService)Activator.CreateInstance(serviceType));
            }
            await service.Initialize();
            return service;
        }

        private static async void DestroyServiceInstance(IService service)
        {
            var serviceType = service.GetType();
            await service.Deinitialize();
            if (serviceType.IsSubclassOf(typeof (Component)))
            {
                var component = (Component) service;
                await UnityAsync.Destroy(component.gameObject);
            }
        }
    }
}
