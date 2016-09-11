using System.Threading.Tasks;
using GameCore.Core.Base.StateMachines;

namespace GameCore.Core.Services.GameState
{
    public class GameStateService : BaseStateMachine<IBaseGameState> //todo iService
    {
        protected override async Task OnBeforeEnterState(IBaseGameState state)
        {
            await state.Load();
        }

        protected override async Task OnAfterExitState(IBaseGameState state)
        {
            await state.Unload();
        }
    }
}
