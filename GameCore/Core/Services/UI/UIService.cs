using System.Collections.Generic;
using System.Threading.Tasks;
using GameCore.Core.Base;
using GameCore.Core.Services.UI.View;
using GameCore.Core.UnityThreading;
using UnityEngine;

namespace GameCore.Core.Services.UI
{
    public class UIService : BaseMonoBehaviour
    {
        private readonly Stack<BaseUIView> _viewsStack = new Stack<BaseUIView>();
        public async Task<TUIView> PushView<TUIView>(GameObject uiViewGameObject,UILayers layer) where TUIView : BaseUIView
        {
            
        }

        public async Task<TViewModel> PushView<TUIView,TViewModel>(GameObject uiViewGameObject, UILayers layer) 
            where TUIView : BaseUIView
            where TViewModel : BaseUIBehaviour
        {
            
        }
        

        public async Task PopView()
        {
            
        }

        public async Task<TUIView> PresentView<TUIView>(GameObject uiViewGameObject, UILayers layer) where TUIView : BaseUIView
        {
            
        }

        public async Task<TViewModel> PresentView<TUIView, TViewModel>()
            where TUIView : BaseUIView
            where TViewModel : BaseUIBehaviour
        {

        }


    }
}
