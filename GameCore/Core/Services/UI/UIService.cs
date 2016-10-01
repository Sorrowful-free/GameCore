using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Core.Extentions;
using GameCore.Core.Base;
using GameCore.Core.Services.UI.Attributes;
using GameCore.Core.Services.UI.View;
using GameCore.Core.Services.UI.ViewModel;


namespace GameCore.Core.Services.UI
{
    public class UIService : BaseMonoBehaviour
    {
        private readonly Dictionary<Type, BaseUIViewModel> _uiViewModelMap = new Dictionary<Type, BaseUIViewModel>();
        private readonly Dictionary<Type, BaseUIView> _uiViewMap = new Dictionary<Type, BaseUIView>();
        private readonly Dictionary<BaseUIViewModel, BaseUIView> _uiMap = new Dictionary<BaseUIViewModel, BaseUIView>();
        private readonly Stack<BaseUIView> _viewStack = new Stack<BaseUIView>();
        public async Task<TView> PushView<TView>(UILayers layer) where TView : BaseUIView
        {
            var uiView = await GetViewInstance<TView>(layer);
            _viewStack.Push(uiView);
            return uiView;
        }

        public async Task<TViewModel> PushView<TView,TViewModel>(UILayers layer) 
            where TView : BaseUIView<TViewModel>
            where TViewModel : BaseUIViewModel,new()
        {
            var uiViewModel = await GetViewInstance<TView, TViewModel>(layer);
            _viewStack.Push(_uiMap[uiViewModel]);
            return uiViewModel;
        }
        
        public async Task PopView() 
        {
            if (_viewStack.Count > 0)
            {
                var lastView = _viewStack.Pop();
                await DestroyView(lastView);
            }
        }

        public async Task<TView> GetViewInstance<TView>(UILayers layer) where TView : BaseUIView
        {
            var type = typeof (TView);
            var uiView = default(BaseUIView);
            if (!_uiViewMap.TryGetValue(type, out uiView))
            {
                var attribute = GetAttribute(typeof(TView));
                var gameObject = await attribute.LoadViewGameObject();
                var uiViewGameObject = await UnityAsync.Instantiate(gameObject);
                uiView = uiViewGameObject.GetComponent<TView>();
                _uiViewMap.Add(type,uiView);
            }
            return (TView)uiView;
        }

        public async Task<TViewModel> GetViewInstance<TView, TViewModel>(UILayers layer)
            where TView : BaseUIView<TViewModel>
            where TViewModel : BaseUIViewModel,new()
        {
            var uiView = await GetViewInstance<TView>(layer);
            var uiViewModel = TryGerOrAddViewModel<TViewModel>();
            _uiMap.Add(uiViewModel,uiView);
            await uiView.Initialize(uiViewModel);
            return uiViewModel;
        }

        public async Task DestroyView(BaseUIView view)
        {
            if (view == null)
            {
                return;
            }
            var type = view.GetType();
            if (_uiViewMap.ContainsKey(type))
            {
                _uiViewMap.Remove(type);
            }
            var attribute = GetAttribute(type);
            await view.Deinitialize();
            await UnityAsync.Destroy(view.gameObject);
            await attribute.UnloadViewGameObject();
        }

        public async Task DestroyView(BaseUIViewModel model)
        {
            var uiView = default(BaseUIView);
            if (!_uiMap.TryGetValue(model, out uiView))
            {
                throw new ArgumentException($"have no view for model {model}");
                return;
            }
            if (_uiMap.ContainsKey(model))
            {
                _uiMap.Remove(model);
            }
            await DestroyView(uiView);
        }

        private TViewModel TryGerOrAddViewModel<TViewModel>() where TViewModel : BaseUIViewModel, new()
        {
            var type = typeof(TViewModel);
            var viewModel = default(BaseUIViewModel);
            if (!_uiViewModelMap.TryGetValue(type, out viewModel))
            {
                viewModel = new TViewModel();
                _uiViewModelMap.Add(type, viewModel);
            }
            return (TViewModel)viewModel;
        }

        private IUIViewLoadAttribute GetAttribute(Type typeOfView)
        {
            var attribute = typeOfView.GetAttributeByInterface<IUIViewLoadAttribute>();
            if (attribute == null)
            {
                throw new ArgumentException("please use attribute implemented from IUIViewAttribute");
            }
            return attribute;
        }
    }
}
