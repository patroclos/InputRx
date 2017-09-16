using System;
using System.Linq;
using UnityEngine;
using UniRx;

namespace InputRx
{
  public abstract class BaseInputConstraint : MonoBehaviour, IInputConstraint
  {
    public IReadOnlyReactiveProperty<bool> IsSatisfied { get { return satisfactionProperty.ToReadOnlyReactiveProperty(); } }

    protected IReactiveProperty<bool> satisfactionProperty = new ReactiveProperty<bool>();

    protected virtual void Awake()
    {
      GetComponents<IInputModule>()
        .Where(module => !module.Constraints.Contains(this))
        .ForEach(module => module.Constraints.Add(this));
    }

    protected virtual void OnDestroy()
    {
      GetComponents<IInputModule>()
        .ForEach(module => module.Constraints.Remove(this));
    }
  }
}