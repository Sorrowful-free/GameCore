using System;
using System.Threading.Tasks;
using UIs.Declaration.Services.ViewModels;
using UnityEngine;

namespace UIs.Declaration.Services.Views
{
    public interface IView : IDisposable
    {
        IViewModel ViewModel { get; }
        Task Initialize(IViewModel viewModel);
        Task DeInitialize();
    }

    public interface IView<TViewModel> : IView
    where TViewModel : IViewModel
    {
        new TViewModel ViewModel { get; }
    }

    public interface IViewRoot
    {
        Transform Root { get; }
    }
}