using System.Threading.Tasks;

namespace GameCore.Core.Application.Interfaces
{
    public interface IService
    {
        Task Initialize();
        Task Deinitialize();
    }
}
