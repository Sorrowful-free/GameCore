using System;
using System.Threading.Tasks;
using Core.Runtime.Services;
using UIs.Declaration.Services.ViewModels;
using UIs.Declaration.Services.Views;

namespace UIs.Declaration.Services
{
    public interface IUIsService : IService
    {
        Task<IViewModel> BindView(IView view, IViewModel viewModel);
        Task<TViewModel> BindView<TView, TViewModel>(TView view, TViewModel viewModel) where TView: IView<TViewModel> where TViewModel : IViewModel; 
        
        Task UnbindView(Type viewModelType);
        Task UnbindView<TViewModel>();
        Task UnbindView(IViewModel viewModel);

        IViewModel GetViewModel(Type viewModelType);
        TViewModel GetViewModel<TViewModel>();
    }
}
