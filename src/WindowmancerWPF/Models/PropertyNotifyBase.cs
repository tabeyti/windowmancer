using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WindowmancerWPF.Models
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

    protected T GetProperty<T>([CallerMemberName]string memeberName = "")
    {
      if (!_props.ContainsKey(memeberName))
      {
        return default(T);
      }
      return (T)_props[memeberName];
    }

    protected void SetProperty(object prop, [CallerMemberName]string memeberName="")
    {
      if (!_props.ContainsKey(memeberName))
      {
        // TODO: warn or error?
        return;
      }

      if (prop != _props[memeberName])
      {
        _props[memeberName] = prop;
        OnPointPropertyChanged(memeberName);
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPointPropertyChanged(string prop)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
  }
}
