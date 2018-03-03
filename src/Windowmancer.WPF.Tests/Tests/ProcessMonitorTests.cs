using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Windowmancer.Core.Models;
using Windowmancer.Core.Practices;
using Windowmancer.Core.Services;
using Windowmancer.Core.Services.Base;
using Windowmancer.WPF.Tests.Practices;
using Windowmancer.WPF.Tests.Tests.Base;
using Xunit;
using Xunit.Abstractions;

namespace Windowmancer.WPF.Tests.Tests
{
  /// <summary>
  /// Validates the API of the ProfileManager service.
  /// </summary>
  public class ProcessMonitorTests : TestClassBase
  {
    private readonly UserData _userData;

    public ProcessMonitorTests(ITestOutputHelper xunitTestOutputHelper) : base(xunitTestOutputHelper)
    {
      _userData = ServiceResolver.Resolve<UserData>();
      _userData.Enable(false);

      Helper.Dispatcher = new TestDispatcher();
    }
    
    [Fact]
    [Trait("Priority", "1")]
    public async Task ProcessMonitorTests_ActiveWindowProcesses_Smoke()
    {
      var processMonitor = ServiceResolver.Resolve<ProcessMonitor>();
      processMonitor.Start();
      await Task.Delay(1000).ConfigureAwait(false);

      Assert.True(processMonitor.ActiveWindowProcs.Count > 0);
    }

    [Fact]
    [Trait("Priority", "2")]
    public async Task ProcessMonitorTests_ActiveWindowProcesses_ProcessAdded()
    {
      var processMonitor = ServiceResolver.Resolve<ProcessMonitor>();

      // Set up our signal for when we receive a new process.
      var signal = new SemaphoreSlim(1);
      await signal.WaitAsync().ConfigureAwait(false);
      processMonitor.OnNewWindowProcess = p => { signal.Release(); };
      processMonitor.Start();
      await Task.Delay(1000).ConfigureAwait(false);

      var onEnter = signal.WaitAsync();
      var onTimeout = Task.Delay(5000);

      // Create a new process and verify it's addition.
      AddResource(TestHelper.CreateWindowProcess());
      var result = await Task.WhenAny(onEnter, onTimeout).ConfigureAwait(false);
      Assert.Equal(onEnter, result);
    }

    [Fact]
    [Trait("Priority", "3")]
    public async Task ProcessMonitorTests_ActiveWindowProcesses_ProcessRemoved()
    {
      var processMonitor = ServiceResolver.Resolve<ProcessMonitor>();

      // Set up our signal for when we receive and remove a process.
      var processNewSignal = new SemaphoreSlim(1);
      var processRemovedSignal = new SemaphoreSlim(1);
      await Task.WhenAll(processNewSignal.WaitAsync(), 
        processRemovedSignal.WaitAsync()).ConfigureAwait(false);

      // Initialize and start our process monitor.
      processMonitor.OnNewWindowProcess = p => { processNewSignal.Release(); };
      processMonitor.OnWindowProcessRemove = p => { processRemovedSignal.Release(); };
      processMonitor.Start();
      await Task.Delay(1000).ConfigureAwait(false);
      
      // Create a new process and wait for it to appear.
      var onEnter = processNewSignal.WaitAsync();
      var proc = AddResource(TestHelper.CreateWindowProcess());
      await onEnter.ConfigureAwait(false);

      // Give a little time in between the creation and removal.
      await Task.Delay(1000).ConfigureAwait(false);

      // Kill the process and verify it's removal.
      var onTimeout = Task.Delay(5000);
      var onRemoved = processRemovedSignal.WaitAsync();
      proc.Kill();
      var result = await Task.WhenAny(onRemoved, onTimeout).ConfigureAwait(false);
      Assert.Equal(onRemoved, result);
    }

    #region Helper Methods

    #endregion Helper Methods
  }

  public class TestDispatcher : IDispatcher
  {
    public void Invoke(Action callback)
    {
      callback();
    }
  }
}
