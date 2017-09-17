using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace InputRx
{
  public class KeyComboConstraint : BaseInputConstraint
  {
    public string[] keys;
    public float inbetweenTime = 0.2f;

    private CompositeDisposable comboBindings = new CompositeDisposable();

    private void Start()
    {
      ListenForCombo();
    }

    private void OnValidate()
    {
      ListenForCombo();
    }

    void ListenForCombo()
    {
#if UNITY_EDITOR
      if (!UnityEditor.EditorApplication.isPlaying)
        return;
#endif
      comboBindings.Clear();

      if (keys.Length == 0)
        return;
      var keyObservables = keys
        .Select((key, index) =>
          Observable
            .EveryUpdate()
            .Where(_ => Input.GetKey(key))
            .Select(_ => Time.time)
            .ToReadOnlyReactiveProperty()
        ).ToArray();

      IReadOnlyReactiveProperty<float> last = keyObservables[0];

      for (int i = 1; i < keys.Length; i++)
      {
        var before = last;
        last = keyObservables[i].SkipUntil(last).Where(_ => Time.time - before.Value <= inbetweenTime).ToReadOnlyReactiveProperty();
      }

      last.Subscribe(_ =>
      {
        satisfactionProperty.Value = true;
        Observable.NextFrame().Subscribe(__ =>
        {
          satisfactionProperty.Value = false;
          ListenForCombo();
        });
      }).AddTo(comboBindings);
    }
  }
}