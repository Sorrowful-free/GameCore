namespace GameCore.Core.Base.StateMachines
{
    public interface IState
    {
        void EnterState(params object[] enterParams);
        void ExitState();
    }
}