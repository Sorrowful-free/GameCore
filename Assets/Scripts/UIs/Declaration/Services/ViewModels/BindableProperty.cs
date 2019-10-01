namespace UIs.Declaration.Services.ViewModels
{
    public delegate void BindablePropertyChangingEvent<TValue>(TValue oldValue, TValue newValue);
    public delegate void BindablePropertyChangeEvent<TValue>(TValue value);
    
    public class BindableProperty<TValue>
    {
        public event BindablePropertyChangingEvent<TValue> OnChanging;
        public event BindablePropertyChangeEvent<TValue> OnChange;

        private TValue _value;

        public TValue Value
        {
            get { return _value; }
            set
            {
                if (!_value.Equals(value))
                {
                    OnChanging?.Invoke(_value,value);
                    _value = value;
                    OnChange?.Invoke(value);
                }
            }
        }

        public BindableProperty(TValue defaultValue = default(TValue))
        {
            _value = defaultValue;
        }
    }
}