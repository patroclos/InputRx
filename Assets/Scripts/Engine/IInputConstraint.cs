using UniRx;

namespace InputRx
{
  public interface IInputConstraint
  {
    IReadOnlyReactiveProperty<bool> IsSatisfied { get; }
  }
}