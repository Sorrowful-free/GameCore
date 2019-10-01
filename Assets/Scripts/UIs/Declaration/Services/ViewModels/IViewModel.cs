using System.Threading.Tasks;

namespace UIs.Declaration.Services.ViewModels
{
    public interface IViewModel
    {
        Task Show();
        Task Hide();
    }
}