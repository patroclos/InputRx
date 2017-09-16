using System;
using System.Linq;
using UnityEngine;
using UniRx;

namespace InputRx
{
  public class TestInputModule : BaseInputModule
  {
    protected override IDisposable BindModule()
    {
      return Observable
        .Interval(TimeSpan.FromSeconds(1))
        .AsUnitObservable()
        .Merge(Observable.ReturnUnit())
        .Subscribe(_ => Debug.Log("I am bound"));
    }
  }
}