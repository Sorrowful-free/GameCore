namespace GameCore.Core.Base.StateMachines
{
    public interface IState
    {
        void EnterState(BaseEnterStateArguments arguments);
        void ExitState();
    }
}