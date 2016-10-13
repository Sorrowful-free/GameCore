using System.Threading.Tasks;

namespace GameCore.Core.Application.Interfaces.Services
{
    public interface IService
    {
        Task Initialize();
        Task Deinitialize();
    }
}
