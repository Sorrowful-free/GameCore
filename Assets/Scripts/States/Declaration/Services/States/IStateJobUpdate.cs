using Unity.Jobs;

namespace States.Declaration.Services.States
{
    public interface IStateJobUpdate
    {
        JobHandle JobUpdate(float deltaTime, JobHandle handle);
    }
}