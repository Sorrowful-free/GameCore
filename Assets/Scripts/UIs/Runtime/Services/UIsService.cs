using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Runtime.Services;
using UIs.Declaration.Services;
using UIs.Declaration.Services.ViewModels;
using UIs.Declaration.Services.Views;
using UnityEngine;

public class UIsService : MonoBehaviour, IUIsService, IServiceRoot
{
    private Dictionary<Type, IViewModel> _viewModels = new Dictionary<Type, IViewModel>();
    private Dictionary<Type, IView> _views = new Dictionary<Type, IView>();

    public Transform Root => transform;
    
    public async void Dispose()
    {
        while (_viewModels.Count>0)
        {
            await UnbindView(_viewModels.Keys.First());
        }
    }

    public Task Initialize()
    {
        return Task.CompletedTask;
    }

    public async Task DeInitialize()
    {
        while (_viewModels.Count>0)
        {
            await UnbindView(_viewModels.Keys.First());
        }
    }

    public async Task<IViewModel> BindView(IView view, IViewModel viewModel)
    {
        _viewModels.Add(viewModel.GetType(),viewModel);
        _views.Add(view.GetType(),view);
        await view.Initialize(viewModel);
        return viewModel;
    }

    public async Task<TViewModel> BindView<TView, TViewModel>(TView view, TViewModel viewModel) where TView : IView<TViewModel> where TViewModel : IViewModel
    {
        return (TViewModel) await BindView((IView)view, (IViewModel)viewModel);
    }

    public async Task UnbindView(Type viewModelType)
    {
        var viewModel = default(IViewModel);
        if (_viewModels.TryGetValue(viewModelType, out viewModel))
        {
            if (_viewModels.Remove(viewModelType))
            {
                await viewModel.Hide();
                var view = default(IView);
                if (_views.TryGetValue(viewModelType, out view))
                {
                    if(_views.Remove(viewModelType))
                        await view.DeInitialize();
                }
            }            
        }
    }

    public async Task UnbindView<TViewModel>()
    {
        await UnbindView(typeof(TViewModel));
    }
    
    public async Task UnbindView(IViewModel viewModel)
    {
        await UnbindView(viewModel.GetType());
    }

    public IViewModel GetViewModel(Type viewModelType)
    {
        var viewModel = default(IViewModel);
        if (_viewModels.TryGetValue(viewModelType, out viewModel))
            return viewModel;
        return null;
    }

    public TViewModel GetViewModel<TViewModel>()
    {
        return (TViewModel) GetViewModel(typeof(TViewModel));
    }

    
}
