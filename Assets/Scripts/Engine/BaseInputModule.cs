using System;
using System.Linq;
using UnityEngine;
using UniRx;

namespace InputRx
{
  public abstract class BaseInputModule : MonoBehaviour, IInputModule
  {
    public IReactiveCollection<IInputConstraint> Constraints { get; private set; }

    private CompositeDisposable bindings = new CompositeDisposable();

    public BaseInputModule()
    {
      Constraints = new ReactiveCollection<IInputConstraint>();
      Constraints.ObserveCountChanged()
        .Subscribe(count => Debug.Log(count));
    }

    protected virtual void Start()
    {
      GetComponents<IInputConstraint>()
        .Where(constraint => !Constraints.Contains(constraint))
        .ForEach(constraint => Constraints.Add(constraint));
    }

    public IDisposable Bind()
    {
      if (!Constraints.All(constraint => constraint.IsSatisfied.Value))
        throw new InvalidOperationException("Can't bind an InputModule with unsatisfied constraints.");

      var binding = BindModule();
      bindings.Add(binding);
      return binding;
    }

    protected virtual void OnDisable()
    {
      bindings.Clear();
    }

    protected virtual void OnDestroy()
    {
      bindings.Dispose();
    }

    protected abstract IDisposable BindModule();
  }
}