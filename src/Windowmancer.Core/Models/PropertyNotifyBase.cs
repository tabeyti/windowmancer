using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;

namespace Windowmancer.Core.Models
{
  public class PropertyNotifyBase : INotifyPropertyChanged
  {
    private readonly Dictionary<string, object> _props = new Dictionary<string, object>();

    protected void RegisterProperty<T>(string propertyName)
    {
      if (_props.ContainsKey(propertyName))
      {
        throw new Exception($"PropertyNotifyBase - Already registered property {propertyName}");
      }
      _props.Add(propertyName, default(T));
    }

    /// <summary>
    /// Registers the property name passed (MUST MATCH THE ACTUAL NAME OF THE
    /// PROPERTY LOUD NOISES!!) for property notify events, assinging the value
    /// provided.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertyName"></param>
    /// <param name="value"></param>
    protected void RegisterProperty<T>(string propertyName, T value)
    {
      if (_props.ContainsKey(propertyName))
      {
        throw new Exception($"PropertyNotifyBase - Already registered property {propertyName}");
      }
      
      // Check if this is an object with PropertyNotifyBase, and if 
      // so, attach to it's PropertyChanged event.
      if (typeof(PropertyNotifyBase).IsAssignableFrom(typeof(T)) && null != value)
      {
        ((PropertyNotifyBase) ((object)value)).PropertyChanged += (sender, args) =>
        {
          OnPointPropertyChanged(propertyName);
        };          
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

    protected void SetProperty<T>(T value, [CallerMemberName] string memberName = "")
    {
      if (!_props.ContainsKey(memberName))
      {
        // TODO: warn or error?
        return;
      }
      if (null != value && value.Equals((T) _props[memberName])) return;
      
      // Add to the properties dictionary.
      _props[memberName] = value;
        
      // Check if this is an object with PropertyNotifyBase, and if 
      // so, attach to it's PropertyChanged event.
      if (typeof(PropertyNotifyBase).IsAssignableFrom(typeof(T)) && null != value)
      {
        ((PropertyNotifyBase) _props[memberName]).PropertyChanged += (sender, args) =>
        {
          OnPointPropertyChanged(memberName);
        };          
      }
      OnPointPropertyChanged(memberName);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    // ReSharper disable once MemberCanBePrivate.Global
    protected void OnPointPropertyChanged(string prop)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
  }
}
