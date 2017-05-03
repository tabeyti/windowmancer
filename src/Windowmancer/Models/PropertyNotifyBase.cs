using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Windowmancer.Models
{
  public class PropertyNotifyBase : INotifyPropertyChanged
  {
    private Dictionary<string, object> _props = new Dictionary<string, object>();

    protected void RegisterProperty<T>(string propertyName)
    {
      if (_props.ContainsKey(propertyName))
      {
        throw new Exception($"PropertyNotifyBase - Already registered property {propertyName}");
      }
      _props.Add(propertyName, default(T));
    }

    protected void RegisterProperty<T>(string propertyName, T value)
    {
      if (_props.ContainsKey(propertyName))
      {
        throw new Exception($"PropertyNotifyBase - Already registered property {propertyName}");
      }
      _props.Add(propertyName, value);
    }

    protected T GetProperty<T>([CallerMemberName]string memberName = "")
    {
      if (!_props.ContainsKey(memberName))
      {
        return default(T);
      }
      return (T)_props[memberName];
    }

    protected void SetProperty(object value, [CallerMemberName]string memberName="")
    {
      if (!_props.ContainsKey(memberName))
      {
        // TODO: warn or error?
        return;
      }

      if (value != _props[memberName])
      {
        _props[memberName] = value;
        OnPointPropertyChanged(memberName);
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPointPropertyChanged(string prop)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
  }
}
