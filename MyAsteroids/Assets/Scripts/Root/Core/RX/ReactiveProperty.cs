using System;

namespace Root.Core.RX
{
    public class ReactiveProperty<T>
    {
        public event Action<T> OnChanged;
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if (!_value.Equals(value))
                {
                    _value = value;
                    OnChanged?.Invoke(_value);
                }
            }
        }
        public ReactiveProperty()
        {
        }

        public ReactiveProperty(T value)
        {
            _value = value;
        }
        public void Invoke()
        {
            OnChanged?.Invoke(_value);
        }

        public void ForceSet(T value)
        {
            _value = value;
            OnChanged?.Invoke(_value);
        }
    }

}