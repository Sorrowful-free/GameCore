using System;
using GameCore.Core.Extentions;
using UnityEngine.Events;

namespace GameCore.Core.Services.UI.ViewModel
{
    public class BindingProperty<TType>
    {
        public event Action<TType> OnChange;

        private TType _value;

        public TType Value
        {
            get { return _value; }
            set
            {
                if (!_value.Equals(value))
                {
                    _value = value;
                    OnChange.SafeInvoke(_value, ActionInvokationType.MainThread);
                }
            }
        }
        
        public void Bind(Action<TType> onChange)
        {
            if (onChange != null)
            {
                OnChange += onChange;
                onChange.SafeInvoke(_value, ActionInvokationType.MainThread); // специально при бинде применить сразу
            }
        }

        public void Bind(Action<TType> onChange, UnityEvent<TType> change)
        {
            Bind(onChange);
            if (change != null)
            {
                change.AddListener(OnChangeHandler);
            }
        }

        public void Bind(Action<TType> onChange, Action<TType> change)
        {
            Bind(onChange);
            if (change != null)
            {
                change += OnChangeHandler;
            }
        }

        public void UnBind(Action<TType> onChange)
        {
            if (onChange != null)
            {
                OnChange -= onChange;
            }
        }

        public void UnBind(Action<TType> onChange, UnityEvent<TType> change)
        {
            UnBind(onChange);
            if (change != null)
            {
                change.RemoveListener(OnChangeHandler);
            }
        }

        public void UnBind(Action<TType> onChange, Action<TType> change)
        {
            UnBind(onChange);
            if (onChange != null)
            {
                OnChange -= onChange;
            }
            if (change != null)
            {
                change-= OnChangeHandler;
            }
        }

        public void ClearBindings()
        {
            OnChange.ClearAllHandlers();
        }

        private void OnChangeHandler(TType value)
        {
            Value = value;
        }

        public static implicit operator TType(BindingProperty<TType> prop)
        {
            return prop.Value;
        }
    }
}
