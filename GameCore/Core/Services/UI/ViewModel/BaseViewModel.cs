namespace GameCore.Core.Services.UI.ViewModel
{
    public class BaseViewModel
    {
        public readonly BindingProperty<bool> Visible = new BindingProperty<bool>();

        public BaseViewModel()
        {
            Visible.Value = false;
        }
    }
}
