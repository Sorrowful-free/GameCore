using Unity.Jobs;

namespace Core.Runtime.Services
{
    public interface IServiceJobLateUpdate
    {
        JobHandle JobLateUpdate(float deltaTime, JobHandle handle);
    }
}