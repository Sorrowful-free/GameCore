using Unity.Jobs;

namespace Core.Runtime.Services
{
    public interface IServiceJobUpdate
    {
        JobHandle JobUpdate(float deltaTime, JobHandle handle);
    }
}