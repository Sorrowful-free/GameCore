using System.Threading.Tasks;

namespace GameCore.Core.Services.Tutorial.Conditions
{
    public interface ITutorialCondition
    {
        Task Wait();
        void Cancel();
    }
}
