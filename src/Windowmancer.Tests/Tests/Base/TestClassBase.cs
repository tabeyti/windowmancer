using Microsoft.Practices.Unity;
using NLog;
using Windowmancer.Tests.Practices;
using Xunit;
using Xunit.Abstractions;

namespace Windowmancer.Tests.Tests.Base
{
  public abstract  class TestClassBase
  {
    protected ILogger Logger { get; }
    protected IUnityContainer ServiceResolver { get; }

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
  }
}
