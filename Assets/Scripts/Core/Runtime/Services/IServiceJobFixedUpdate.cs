using Unity.Jobs;

namespace Core.Runtime.Services
{
    public interface IServiceJobFixedUpdate
    {
        JobHandle JobFixedUpdate(float fixedDeltaTime, JobHandle handle);
    }
}