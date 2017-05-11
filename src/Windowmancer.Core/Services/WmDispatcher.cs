using System;
using System.Windows.Threading;
using Windowmancer.Core.Services.Base;

namespace Windowmancer.Core.Services
{
  public class WmDispatcher : IDispatcher
  {
    private readonly Dispatcher _dispatcher;

    public WmDispatcher(Dispatcher dispatcher)
    {
      _dispatcher = dispatcher;
    }

    public void Invoke(Action callback)
    {
      _dispatcher?.Invoke(callback);
    }
  }
}
