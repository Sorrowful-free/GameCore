using System;
using System.Threading.Tasks;
using GameCore.Core.Extentions;
using GameCore.Core.Services.UI.ViewModel;

namespace GameCore.Core.Services.UI.View
{
    public abstract class BaseUIView : BaseUIBehaviour
    {
        public event Action<bool> OnVisibleChange;
        public bool Visible
        {
            get { return gameObject.activeSelf; }
            set
            {
                gameObject.SetActive(value);
                OnVisibleChange.SafeInvoke(value);
            }
        }

        public async Task Deinitialize()
        {
            await OnDeinitialize();
        }

        protected abstract Task OnDeinitialize();
    }

    public abstract class BaseUIView<TViewModel> : BaseUIView where TViewModel : BaseUIViewModel
    {
        public TViewModel ViewModel { get; private set; }

        public async Task Initialize(TViewModel viewModel)
        {
            ViewModel = viewModel;
            ViewModel.Visible.Bind((visible) => Visible = visible);
            await OnInitialize();
        }
        
        protected abstract Task OnInitialize();

       
    } 
}
