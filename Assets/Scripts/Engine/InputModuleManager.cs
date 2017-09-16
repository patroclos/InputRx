using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace InputRx
{
  public class InputModuleManager : MonoBehaviour
  {
    [InspectorDisplay]
    public TransformReactiveProperty inputContainer;

    private CompositeDisposable disposable = new CompositeDisposable();

    void OnEnable()
    {
      CompositeDisposable containerSession = new CompositeDisposable();
      inputContainer
        .Subscribe(container =>
        {
          Debug.Log("changed container to " + container);

          container.OnTransformChildrenChangedAsObservable()
            .Merge(Observable.ReturnUnit())
            .Subscribe(delegate
            {
              Debug.Log("Rebinding");
              containerSession.Clear();
              var containerBinding = SetupModules(container.GetComponentsInChildren<IInputModule>())
                .AddTo(containerSession).AddTo(disposable);
              containerBinding.AddTo(containerSession);
            }).AddTo(disposable);
        })
        .AddTo(disposable);
    }

    IDisposable SetupModules(IInputModule[] modules)
    {
      CompositeDisposable subscriptions = new CompositeDisposable();

      foreach (IInputModule module in modules)
      {
        var updateSubject = new Subject<bool>();

        updateSubject
          .DistinctUntilChanged()
          .WhereEquals(true)
          .Subscribe(canBind =>
          {
            var bind = module.Bind();
            bind.AddTo(subscriptions);
            updateSubject.WhereEquals(false).Take(1).Subscribe(_ => bind.Dispose()).AddTo(subscriptions);
          }).AddTo(subscriptions);

        module.Constraints
          .ObserveCountChanged(true)
          .Subscribe(delegate
          {
            updateSubject.OnNext(ConstraintsAreSatisfied(module.Constraints));

            module.Constraints
              .Select(constraint => constraint.IsSatisfied.AsObservable())
              .Merge()
              .TakeUntil(module.Constraints.ObserveCountChanged())
              .Subscribe(delegate
              {
                updateSubject.OnNext(ConstraintsAreSatisfied(module.Constraints));
              });
          })
          .AddTo(subscriptions);

      }

      return subscriptions;
    }

    bool ConstraintsAreSatisfied(IEnumerable<IInputConstraint> constraints)
    {
      return constraints.All(constraint => constraint.IsSatisfied.Value);
    }

    void OnDisable()
    {
      disposable.Clear();
    }

    void OnDestroy()
    {
      disposable.Dispose();
    }
  }


  [Serializable]
  public class TransformReactiveProperty : ReactiveProperty<Transform> { }
}