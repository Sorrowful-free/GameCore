using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Core.Extentions;
using GameCore.Core.Base;
using GameCore.Core.Services.UI.Attributes;
using GameCore.Core.Services.UI.View;
using GameCore.Core.Services.UI.ViewModel;
using GameCore.Core.UnityThreading;
using UnityEngine;

namespace GameCore.Core.Services.UI
{
    public class UIService : BaseMonoBehaviour
    {
        private readonly Stack<BaseUIView> _viewsStack = new Stack<BaseUIView>();
        public async Task<TUIView> PushView<TUIView>(UILayers layer) where TUIView : BaseUIView
        {
            var attribute = await GetAttribute(typeof(TUIView));
            var gameObject = await attribute.LoadViewGameObject();
            var uiViewGameObject = await UnityAsync.Instantiate(gameObject);
            var uiView = uiViewGameObject.GetComponent<TUIView>();
            _viewsStack.Push(uiView);
            return uiView;
        }

        public async Task<TViewModel> PushView<TUIView,TViewModel>(UILayers layer) 
            where TUIView : BaseUIView<TViewModel>
            where TViewModel : BaseViewModel,new()
        {
            var attribute = await GetAttribute(typeof(TUIView));
            var gameObject = await attribute.LoadViewGameObject();
            var uiViewGameObject = await UnityAsync.Instantiate(gameObject);
            var uiView = uiViewGameObject.GetComponent<TUIView>();
            var uiViewModel = new TViewModel();
            uiView.BindViewModel(uiViewModel);
            _viewsStack.Push(uiView);
            return uiViewModel;
        }
        

        public async Task PopView()
        {
            var uiView = _viewsStack.Pop(); 
            throw new NotImplementedException();
        }

        public async Task<TUIView> PresentView<TUIView>(UILayers layer) where TUIView : BaseUIView
        {
            var attribute = await GetAttribute(typeof(TUIView));
            var gameObject = await attribute.LoadViewGameObject();
            var uiViewGameObject = await UnityAsync.Instantiate(gameObject);
            var uiView = uiViewGameObject.GetComponent<TUIView>();
            return uiView;
        }

        public async Task<TViewModel> PresentView<TUIView, TViewModel>(UILayers layer)
            where TUIView : BaseUIView<TViewModel>
            where TViewModel : BaseViewModel,new()
        {
            var attribute = await GetAttribute(typeof (TUIView));
            var gameObject = await attribute.LoadViewGameObject();
            var uiViewGameObject = await UnityAsync.Instantiate(gameObject);
            var uiView = uiViewGameObject.GetComponent<TUIView>();
            var uiViewModel = new TViewModel();
            uiView.BindViewModel(uiViewModel);
            return uiViewModel;
        }

        private async Task<IUIViewAttribute> GetAttribute(Type typeOfView)
        {
            var attribute = await
                UnityTask<IUIViewAttribute>.Factory.StartNew(
                    () => typeOfView.GetAttributeByInterface<IUIViewAttribute>());
            if (attribute == null)
            {
                throw new ArgumentException("please use attribute implemented from IUIViewAttribute");
            }
            return attribute;
        }




    }
}
