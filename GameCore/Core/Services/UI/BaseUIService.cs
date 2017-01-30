﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameCore.Core.Application.Interfaces.Services;
using GameCore.Core.Base;
using GameCore.Core.Base.Async;
using GameCore.Core.Base.Attributes;
using GameCore.Core.Extentions;
using GameCore.Core.Logging;
using GameCore.Core.Services.UI.Layers;
using GameCore.Core.Services.UI.Utils;
using GameCore.Core.Services.UI.View;
using GameCore.Core.Services.UI.ViewModel;
using GameCore.Core.UnityThreading;
using UnityEngine;


namespace GameCore.Core.Services.UI
{
    public abstract class BaseUIService : BaseMonoBehaviour, IService
    {
        public abstract List<UILayerInfo> LayerInfos { get; }
        public abstract Task Initialize();

        public abstract Task Deinitialize();

        [SerializeField, Tooltip("in inches")]
        private float _minScreenDiagonal;
        [SerializeField]
        private Vector2 _minScreenResolution;
        [SerializeField, Tooltip("in inches")]
        private float _maxScreenDiagonal;
        [SerializeField]
        private Vector2 _maxScreenResolution;
        [SerializeField, Tooltip("x axis = screen diagonal \n y axis = scale factor")]
        private AnimationCurve _scaleFactor;

        [Header("Debug"), SerializeField, Tooltip("set diagonal only in editor"), Range(0, 15)]
        private float _screenDiagonal;

        protected override void Awake()
        {
            base.Awake();
            if (!UnityEngine.Application.isEditor)
                Recalculate(UIUtils.GetPhysicalScreenDiagonal());
            else
                Recalculate(_screenDiagonal);
        }

        private void OnValidate()
        {
            if (UnityEngine.Application.isEditor)
                Recalculate(_screenDiagonal);
        }

        private void Recalculate(float screenDiagonal)
        {
            screenDiagonal = Mathf.Clamp(screenDiagonal, _minScreenDiagonal, _maxScreenDiagonal);
            var normalizedScreenDiagonal = (screenDiagonal - _minScreenDiagonal) / (_maxScreenDiagonal - _minScreenDiagonal);
            var scaleFactor = _scaleFactor.Evaluate(normalizedScreenDiagonal);
            var currentCanvasScalerResolutin = _minScreenResolution + (_maxScreenResolution - _minScreenResolution) * scaleFactor;
            foreach (var layer in LayerInfos)
            {
                layer.Layer.CanvasScaler.referenceResolution = currentCanvasScalerResolutin;
            }
        }
    }
    public abstract class BaseUIService<TUILayerType> :BaseUIService
        where TUILayerType : struct
    {
        public override List<UILayerInfo> LayerInfos {
            get { return _layersInfos; }
        }

        [SerializeField,HideInInspector]
        private List<UILayerInfo> _layersInfos = new List<UILayerInfo>();

        private readonly Dictionary<Type, BaseUIViewModel> _uiViewModelMap = new Dictionary<Type, BaseUIViewModel>();
        private readonly Dictionary<Type, BaseUIView> _uiViewMap = new Dictionary<Type, BaseUIView>();
        private readonly Dictionary<BaseUIViewModel, BaseUIView> _uiMap = new Dictionary<BaseUIViewModel, BaseUIView>();
        private readonly Stack<BaseUIView> _viewStack = new Stack<BaseUIView>();
        private readonly Dictionary<TUILayerType, UILayer> _layersMap = new Dictionary<TUILayerType, UILayer>();

        public override async Task Initialize()
        {
            await UnityTask.MainThreadFactory.StartNew(() =>
            {
                if(_layersInfos != null)
                foreach (var info in _layersInfos)
                {
                    _layersMap.Add((TUILayerType) (object) info.LayerType, info.Layer);
                }
            });
        }

        public override async Task Deinitialize()
        {
        }

        public async Task<TView> PushView<TView>(TUILayerType layerType) where TView : BaseUIView
        {
            var uiView = await CreateView<TView>(layerType);
            _viewStack.Push(uiView);
            return uiView;
        }

        public async Task<TViewModel> PushView<TView,TViewModel>(TUILayerType layerType) 
            where TView : BaseUIView<TViewModel>
            where TViewModel : BaseUIViewModel,new()
        {
            var uiViewModel = await CreateView<TView, TViewModel>(layerType);
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

        public async Task<TView> CreateView<TView>(TUILayerType layerType) where TView : BaseUIView
        {
            var type = typeof (TView);
            var uiView = default(BaseUIView);
            if (!_uiViewMap.TryGetValue(type, out uiView))
            {
                var attribute = GetAttribute(typeof(TView));
                var gameObject = await attribute.LoadGameObject();
                var uiViewGameObject = await UnityAsync.Instantiate(gameObject);
                uiView = uiViewGameObject.GetComponent<TView>();
                _uiViewMap.Add(type,uiView);
                _layersMap[layerType].AddChild(uiView);
            }
            return (TView)uiView;
        }

        public async Task<TViewModel> CreateView<TView, TViewModel>(TUILayerType layerType)
            where TView : BaseUIView<TViewModel>
            where TViewModel : BaseUIViewModel,new()
        {
            var uiView = await CreateView<TView>(layerType);
            var uiViewModel = TryGerOrAddViewModel<TViewModel>();
            _uiMap.Add(uiViewModel,uiView);
            await uiView.Initialize(uiViewModel);
            _layersMap[layerType].AddChild(uiView);
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
            var layer = _layersMap.FirstOrDefault(e => e.Value.ContainChild(view)).Value;
            layer.RemoveChild(view);
            await UnityAsync.Destroy(view.gameObject);
            await attribute.UnloadGameObject();
        }

        public async Task DestroyView(BaseUIViewModel model)
        {
            var uiView = default(BaseUIView);
            if (!_uiMap.TryGetValue(model, out uiView))
            {
                throw new ArgumentException($"have no view for model {model}");
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

        private IGameObjectLoadAttribute GetAttribute(Type typeOfView)
        {
            var attribute = typeOfView.GetAttributeByInterface<IGameObjectLoadAttribute>();
            if (attribute == null)
            {
                throw new ArgumentException("please use attribute implemented from IGameObjectLoadAttribute");
            }
            return attribute;
        }
    }

    [Serializable]
    public class UILayerInfo
    {
        public int LayerType;
        public UILayer Layer;
    }
}
