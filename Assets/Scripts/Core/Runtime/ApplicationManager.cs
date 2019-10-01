using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Modules;
using UnityEngine;

namespace Core.Runtime
{
    public partial class ApplicationManager : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod]
        private static async void StartupApplication()
        {
            if (Current == null)
            {
                Current = new GameObject(nameof(ApplicationManager)).AddComponent<ApplicationManager>();
                DontDestroyOnLoad(Current.gameObject);
                await Current.Initialize();
            }
        }

        public static ApplicationManager Current { get; private set; }

        public ApplicationManagerDelegate ApplicationManagerDelegate { get; private set; }

        private async Task Initialize()
        {
            var allApplicationManagerDelegates = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(asm => asm.GetTypes())
                .Where(e => e.IsSubclassOf(typeof(ApplicationManagerDelegate)) && !e.IsAbstract);

            if (!allApplicationManagerDelegates.Any())
                throw new ArgumentException($"please extends one class from {nameof(ApplicationManagerDelegate)}");
            
            if (allApplicationManagerDelegates.Count() > 1)
                throw new ArgumentException(
                    $"please remove unused {nameof(ApplicationManagerDelegate)} you have {string.Join("\n", allApplicationManagerDelegates)}");
            
            ApplicationManagerDelegate =(ApplicationManagerDelegate)
                Activator.CreateInstance(allApplicationManagerDelegates.First(), new object[] {this});

            await ApplicationManagerDelegate.OnApplicationStart();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            ModulesOnApplicationFocus(hasFocus);
            ApplicationManagerDelegate?.OnApplicationFocus(hasFocus);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            ModulesOnApplicationPause(pauseStatus);
            ApplicationManagerDelegate?.OnApplicationPause(pauseStatus);
        }

        private void OnApplicationQuit()
        {
            ModulesOnApplicationQuit();
            ApplicationManagerDelegate?.OnApplicationQuit();
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            ServicesUpdate(deltaTime);
            ServicesJobsUpdate(deltaTime);
        }

        private void LateUpdate()
        {
            var deltaTime = Time.deltaTime;
            ServicesLateUpdate(deltaTime);
            ServicesJobsLateUpdate(deltaTime);
            
        }
        
        private void FixedUpdate()
        {
            var fixedDeltaTime = Time.fixedDeltaTime;
            ServicesFixedUpdate(fixedDeltaTime);   
            ServicesJobsFixedUpdate(fixedDeltaTime);
        }
    }
}