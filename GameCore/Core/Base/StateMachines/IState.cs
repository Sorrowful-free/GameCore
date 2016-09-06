using System.Threading.Tasks;

namespace GameCore.Core.Base.StateMachines
{
    public interface IState
    {
        Task Preload();
        Task Unload();  
        void EnterState(params object[] enterParams);
        void ExitState();
    }
}