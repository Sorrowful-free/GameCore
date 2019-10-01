using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Runtime.Services;
using Unity.Jobs;
using UnityEngine;

namespace Core.Runtime
{
    public partial class ApplicationManager
    {
        private Dictionary<Type, IService> _services = new Dictionary<Type, IService>();
        
        private Dictionary<Type, IServiceUpdate> _servicesUpdates = new Dictionary<Type, IServiceUpdate>();
        private IEnumerable<IServiceUpdate> _internalServicesUpdates;
        
        private Dictionary<Type, IServiceLateUpdate> _servicesLateUpdates = new Dictionary<Type, IServiceLateUpdate>();
        private IEnumerable<IServiceLateUpdate> _internalServicesLateUpdates;
        
        private Dictionary<Type, IServiceFixedUpdate> _servicesFixedUpdates = new Dictionary<Type, IServiceFixedUpdate>();
        private IEnumerable<IServiceFixedUpdate> _internalServicesFixedUpdates;
        
        private Dictionary<Type, IServiceJobUpdate> _servicesJobUpdates = new Dictionary<Type, IServiceJobUpdate>();
        private IEnumerable<IServiceJobUpdate> _internalServicesJobUpdates;
        
        private Dictionary<Type, IServiceJobLateUpdate> _servicesJobLateUpdates = new Dictionary<Type, IServiceJobLateUpdate>();
        private IEnumerable<IServiceJobLateUpdate> _internalServicesJobLateUpdates;
        
        private Dictionary<Type, IServiceJobFixedUpdate> _servicesJobFixedUpdates = new Dictionary<Type, IServiceJobFixedUpdate>();
        private IEnumerable<IServiceJobFixedUpdate> _internalServicesJobFixedUpdates;

        public async Task<IService> InitializeService(Type serviceDeclarationType, IService serviceImplementation)
        {
            _services.Add(serviceDeclarationType, serviceImplementation);
            
            await serviceImplementation.Initialize();

            if (serviceImplementation is IServiceUpdate)
            {
                _servicesUpdates.Add(serviceDeclarationType,(IServiceUpdate)serviceImplementation);
                _internalServicesUpdates = _servicesUpdates.Values.ToArray();
            }
            
            if (serviceImplementation is IServiceLateUpdate)
            {
                _servicesLateUpdates.Add(serviceDeclarationType,(IServiceLateUpdate)serviceImplementation);
                _internalServicesLateUpdates = _servicesLateUpdates.Values.ToArray();
            }
            
            if (serviceImplementation is IServiceFixedUpdate)
            {
                _servicesFixedUpdates.Add(serviceDeclarationType,(IServiceFixedUpdate)serviceImplementation);
                _internalServicesFixedUpdates = _servicesFixedUpdates.Values.ToArray();
            }
            
            if (serviceImplementation is IServiceJobUpdate)
            {
                _servicesJobUpdates.Add(serviceDeclarationType,(IServiceJobUpdate)serviceImplementation);
                _internalServicesJobUpdates = _servicesJobUpdates.Values.ToArray();
            }
            
            if (serviceImplementation is IServiceJobLateUpdate)
            {
                _servicesJobLateUpdates.Add(serviceDeclarationType,(IServiceJobLateUpdate)serviceImplementation);
                _internalServicesJobLateUpdates = _servicesJobLateUpdates.Values.ToArray();
            }
            
            if (serviceImplementation is IServiceJobFixedUpdate)
            {
                _servicesJobFixedUpdates.Add(serviceDeclarationType,(IServiceJobFixedUpdate)serviceImplementation);
                _internalServicesJobFixedUpdates = _servicesJobFixedUpdates.Values.ToArray();
            }

            if (serviceImplementation is IServiceRoot)
            {
                ((IServiceRoot)serviceImplementation).Root.SetParent(transform);
            }
            
            return serviceImplementation;
        }

        public async Task<TServiceDeclaration> InitializeService<TServiceDeclaration>(
            TServiceDeclaration serviceImplementation)
            where TServiceDeclaration : IService
        {
            return (TServiceDeclaration) await InitializeService(typeof(TServiceDeclaration), serviceImplementation);
        }

        public async Task DeInitializeService(Type serviceDeclarationType)
        {
            var serviceImplementation = default(IService);
            if (_services.TryGetValue(serviceDeclarationType, out serviceImplementation))
            {
                if (_servicesUpdates.Remove(serviceDeclarationType))
                {
                    _internalServicesUpdates = _servicesUpdates.Values.ToArray();
                }

                if (_servicesLateUpdates.Remove(serviceDeclarationType))
                {
                    _internalServicesLateUpdates = _servicesLateUpdates.Values.ToArray();
                }

                if (_servicesFixedUpdates.Remove(serviceDeclarationType))
                {
                    _internalServicesFixedUpdates = _servicesFixedUpdates.Values.ToArray();
                }

                if (_servicesJobUpdates.Remove(serviceDeclarationType))
                {
                    _internalServicesJobUpdates = _servicesJobUpdates.Values.ToArray();
                }
                
                if (_servicesJobLateUpdates.Remove(serviceDeclarationType))
                {
                    _internalServicesJobLateUpdates = _servicesJobLateUpdates.Values.ToArray();
                }
                
                if (_servicesJobFixedUpdates.Remove(serviceDeclarationType))
                {
                    _internalServicesJobFixedUpdates = _servicesJobFixedUpdates.Values.ToArray();
                }

                if (_services.Remove(serviceDeclarationType))
                {
                    await serviceImplementation.DeInitialize();
                    serviceImplementation.Dispose();
                }
                
                if (serviceImplementation is IServiceRoot)
                {
                    Destroy(((IServiceRoot)serviceImplementation).Root.gameObject);
                }
            }
        }

        public Task DeInitializeService<TServiceDeclaration>()
            where TServiceDeclaration : IService
        {
            return DeInitializeService(typeof(TServiceDeclaration));
        }

        public IService GetService(Type serviceDeclarationType)
        {
            var service = default(IService);
            if(_services.TryGetValue(serviceDeclarationType, out service))
            {
                return service;
            }
            return null;
        }

        public TServiceDeclaration GetService<TServiceDeclaration>() where TServiceDeclaration : IService
        {
            return (TServiceDeclaration) GetService(typeof(TServiceDeclaration));
        }

        private void ServicesUpdate(float deltaTime)
        {
            if(_internalServicesUpdates == null)
                return;
            foreach (var serviceUpdate in _internalServicesUpdates)
            {
                serviceUpdate.Update(deltaTime);
            }
        }

        private void ServicesLateUpdate(float deltaTime)
        {
            if(_internalServicesLateUpdates == null)
                return;
            foreach (var serviceLateUpdate in _internalServicesLateUpdates)
            {
                serviceLateUpdate.LateUpdate(deltaTime);
            }
        }

        private void ServicesFixedUpdate(float fixedDeltaTime)
        {
            if(_internalServicesFixedUpdates == null)
                return;
            foreach (var serviceFixedUpdate in _internalServicesFixedUpdates)
            {
                serviceFixedUpdate.FixedUpdate(fixedDeltaTime);
            }
        }

        private void ServicesJobsUpdate(float deltaTime)
        {
            if(_internalServicesJobUpdates == null)
                return;
            var handle = default(JobHandle);
            foreach (var serviceJobUpdate in _internalServicesJobUpdates)
            {
                handle = serviceJobUpdate.JobUpdate(deltaTime, handle);
            }
            handle.Complete();
        }
        
        private void ServicesJobsLateUpdate(float deltaTime)
        {
            if(_internalServicesJobLateUpdates == null)
                return;
            var handle = default(JobHandle);
            foreach (var serviceJobLateUpdate in _internalServicesJobLateUpdates)
            {
                handle = serviceJobLateUpdate.JobLateUpdate(deltaTime, handle);
            }
            handle.Complete();
        }
        
        private void ServicesJobsFixedUpdate(float fixedDeltaTime)
        {
            if(_internalServicesJobFixedUpdates == null)
                return;
            var handle = default(JobHandle);
            foreach (var serviceJobFixedUpdate in _internalServicesJobFixedUpdates)
            {
                handle = serviceJobFixedUpdate.JobFixedUpdate(fixedDeltaTime, handle);
            }
            handle.Complete();
        }
    }
}