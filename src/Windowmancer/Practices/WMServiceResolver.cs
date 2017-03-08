using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windowmancer.Configuration;

namespace Windowmancer.Pratices
{
  public static class WMServiceResolver
  {
    private static IUnityContainer _instance;
    public static IUnityContainer Instance
    {
      get
      {
        return _instance == null ? _instance = CreateResolver() : _instance;
      }
    }

    private static IUnityContainer CreateResolver()
    {
      var container = new UnityContainer();
      var config = GetAssembly();

      // Register configs.
      RegisterConfig<ProfileManagerConfig>(container, config.AppSettings);

      // Register all services.
      RegisterServices(container);

      return container;
    }

    private static void RegisterServices(IUnityContainer container)
    {
      // TODO: Move service registrations here.
    }

    public static dynamic GetAssembly()
    {
      return JsonConvert.DeserializeObject(File.ReadAllText($"{System.Reflection.Assembly.GetCallingAssembly().GetName().Name}.json"));
    }

    private static void RegisterConfig<T>(IUnityContainer container, dynamic appSettings)
    {
      T instance = RegisterConfig<T>(appSettings);

      // Register instance.
      container.RegisterInstance(typeof(T), instance);
    }

    public static T InstanceFromConfig<T>(dynamic config)
    {
      var instance = Activator.CreateInstance(typeof(T));

      if (null == config)
      {
        return (T)instance;
      }

      foreach (var p in config.Properties())
      {
        PropertyInfo property = instance.GetType().GetProperty(p.Name, BindingFlags.Public | BindingFlags.Instance);
        if (null == property || !property.CanWrite) continue;
        try
        {
          var value = Convert.ChangeType(p.Value.Value, property.PropertyType);
          property.SetValue(instance, value, null);
        }
        catch (Exception e)
        {
          Console.WriteLine($"WARNING: Issue applying config value {p.Value.Value} to {instance.GetType().Name}.{property.Name}({property.PropertyType}): {e.Message}");
        }
      }
      return (T)instance;
    }

    public static T RegisterConfig<T>(dynamic appSettings)
    {
      // Attempt to locate associate config class within the config json.
      dynamic prop = null;
      foreach (var p in appSettings.Properties())
      {
        if (p.Name != typeof(T).Name) continue;
        prop = p;
        break;
      }

      return InstanceFromConfig<T>(prop?.Value);
    }
  }
}
