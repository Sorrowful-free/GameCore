using Unity.Jobs;

namespace States.Declaration.Services.States
{
    public interface IStateJobFixedUpdate
    {
        JobHandle JobFixedUpdate(float fixedDeltaTime, JobHandle handle);
    }
}