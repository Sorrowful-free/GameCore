using Unity.Jobs;

namespace States.Declaration.Services.States
{
    public interface IStateJobLateUpdate
    {
        JobHandle JobLateUpdate(float deltaTime, JobHandle handle);
    }
}