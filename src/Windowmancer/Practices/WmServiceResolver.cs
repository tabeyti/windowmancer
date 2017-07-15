using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using Windowmancer.Core.Configuration;
using Windowmancer.Core.Models;
using Windowmancer.Core.Practices;
using Windowmancer.Core.Services;

namespace Windowmancer.Practices
{
  public static class WmServiceResolver
  {
    private static IUnityContainer _instance;
    public static IUnityContainer Instance => _instance ?? (_instance = CreateResolver());

    private static IUnityContainer CreateResolver()
    {
      var container = new UnityContainer();
      var config = Helper.GetConfig("Windowmancer.json");

      // Set Helper instance of our service resolver.
      Helper.ServiceResolver = container;

      // Register configs.
      RegisterConfig<UserConfig>(container, config.AppSettings);

      // Register all services.
      RegisterServices(container);

      return container;
    }

    private static void RegisterServices(IUnityContainer container)
    {
      var userConfig = container.Resolve<UserConfig>();
      if (!File.Exists(userConfig.UserDataPath))
      {
        File.WriteAllText(userConfig.UserDataPath, JsonConvert.SerializeObject(new UserData(null)));
      }          
      var text = File.ReadAllText(userConfig.UserDataPath);
      var userData = JsonConvert.DeserializeObject<UserData>(text);
      userData.SetUserConfig(userConfig);
      container.RegisterInstance(userData, new ContainerControlledLifetimeManager());
      container.RegisterType<ProcessMonitor>(new ContainerControlledLifetimeManager());
      container.RegisterType<MonitorWindowManager>(new ContainerControlledLifetimeManager());
      container.RegisterType<ProfileManager>(new ContainerControlledLifetimeManager());
      container.RegisterType<KeyHookManager>(new ContainerControlledLifetimeManager());
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
