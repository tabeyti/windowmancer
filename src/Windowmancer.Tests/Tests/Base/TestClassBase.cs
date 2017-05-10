﻿using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using NLog;
using Windowmancer.Tests.Practices;
using Xunit;
using Xunit.Abstractions;

namespace Windowmancer.Tests.Tests.Base
{
  public abstract  class TestClassBase : IDisposable
  {
    protected ILogger Logger { get; }
    protected IUnityContainer ServiceResolver { get; }

    private List<IDisposable> _resources = new List<IDisposable>();

    /// <summary>
    /// Initializes a new instance of the <see cref="TestClassBase" /> class.
    /// </summary>
    protected TestClassBase(ITestOutputHelper xunitTestOutputHelper)
    {
      this.ServiceResolver = WmServiceResolver.Instance;

      this.Logger = xunitTestOutputHelper?.GetNLogLogger();

      if (null == Logger)
      {
        this.Logger = this.ServiceResolver.Resolve<ILogger>();
      }
    }

    protected T AddResource<T>(T resource)
      where T : IDisposable
    {
      _resources.Add(resource);
      return resource;
    }

    public void Dispose()
    {
      foreach (var r in _resources)
      {
        r.Dispose();
      }
    }
  }
}
