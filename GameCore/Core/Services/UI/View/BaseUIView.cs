using GameCore.Core.Services.UI.ViewModel;

namespace GameCore.Core.Services.UI.View
{
    public abstract class BaseUIView : BaseUIBehaviour
    {
        public bool Visible
        {
            get { return gameObject.activeSelf; }
            set { gameObject.SetActive(value); }
        }
    }

    public abstract class BaseUIView<TViewModel> : BaseUIView where TViewModel : BaseViewModel
    {
        public TViewModel ViewModel { get; private set; }

        public void InitializeViewModel(TViewModel viewModel)
        {
            ViewModel = viewModel;
            ViewModel.Visible.Bind((visible)=>Visible = visible);
            OnInitialize();
        }

        public void Deinitialize()
        {
            OnDeinitialize();
        }

        protected abstract void OnInitialize();

        protected abstract void OnDeinitialize();
    } 
}
