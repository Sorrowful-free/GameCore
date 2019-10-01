using System;
using System.Threading.Tasks;

namespace Core.Runtime.Services
{
    public interface IService : IDisposable
    {
        Task Initialize();
        Task DeInitialize();
    }
}