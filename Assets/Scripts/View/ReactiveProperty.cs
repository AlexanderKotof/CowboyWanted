using System;

public class ReactiveProperty<T> : IDisposable where T : struct
{
    public T Value { 
        get 
        { 
            return _value; 
        }
        set
        {
            SetValue(value);
        }
    }
    private T _value;

    public event Action<T> OnChanged;

    public void SetValue(T val)
    {
        _value = val;
        OnChanged?.Invoke(_value);
    }

    public void Subscribe(Action<T> onChanged)
    {
        OnChanged += onChanged;
    }

    public void Unsubscribe(Action<T> onChanged)
    {
        OnChanged -= onChanged;
    }

    public void RemoveAllSubscribers()
    {
        OnChanged = null;
    }

    public void Dispose()
    {
        RemoveAllSubscribers();
        _value = default;
    }
}
