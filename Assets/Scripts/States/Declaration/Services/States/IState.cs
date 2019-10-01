using System;
using System.Threading.Tasks;

namespace States.Declaration.Services.States
{
    public interface IState : IDisposable
    {
        Task EnterState(params object[] enterArgs);
        Task ExitState();
    }
}