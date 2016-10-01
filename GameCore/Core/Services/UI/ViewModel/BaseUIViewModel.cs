namespace GameCore.Core.Services.UI.ViewModel
{
    public class BaseUIViewModel
    {
        public readonly BindingProperty<bool> Visible = new BindingProperty<bool>();

        public BaseUIViewModel()
        {
            Visible.Value = false;
        }
    }
}
