using System;
using UniRx;

namespace InputRx
{
  public interface IInputModule
  {
    IDisposable Bind();
    IReactiveCollection<IInputConstraint> Constraints { get; }
  }
}