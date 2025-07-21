using System;
using System.Collections.Generic;

namespace CodeBase.Utils
{
  public class Observable<T>
  {
    private T _value;
    public event Action<T> OnChanged;

    public T Value
    {
      get => _value;
      set
      {
        if (EqualityComparer<T>.Default.Equals(_value, value))
          return;
        
        _value = value;
        Invoke();
      }
    }

    public Observable(T value)
    {
      _value = value;
    }

    private void Invoke() => 
      OnChanged?.Invoke(_value);
  }
}