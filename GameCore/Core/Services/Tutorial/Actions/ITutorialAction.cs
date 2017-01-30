using System.Threading.Tasks;

namespace GameCore.Core.Services.Tutorial.Actions
{
    public interface ITutorialAction
    {
        Task Begin();
        Task End();
    }
}
