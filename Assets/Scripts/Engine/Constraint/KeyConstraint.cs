using System;
using System.Linq;
using UnityEngine;
using UniRx;

namespace InputRx
{
  public class KeyConstraint : BaseInputConstraint
  {
    public enum EventType
    {
      Pressed,
      Up,
      Down
    }

    [Serializable]
    public class EventTypeReactiveProperty : ReactiveProperty<EventType> { }

    public StringReactiveProperty key = new StringReactiveProperty();
    [InspectorDisplay]
    public EventTypeReactiveProperty eventType = new EventTypeReactiveProperty();

    private void Start()
    {
      var updateSignal = Observable.Merge(key.AsUnitObservable(), eventType.AsUnitObservable());

      var disposable = new CompositeDisposable();
      updateSignal
      .Subscribe(delegate
      {
        disposable.Clear();
        if (Enum.GetNames(typeof(KeyCode)).Contains(key.Value))
          Observable.EveryUpdate()
            .Select(GetKeySelector<long>(key.Value, eventType.Value))
            .Subscribe(isSatisfied => satisfactionProperty.Value = isSatisfied)
            .AddTo(disposable);
      });
    }

    private Func<T, bool> GetKeySelector<T>(string key, EventType type)
    {
      Func<bool> selector = null;
      var keycode = (KeyCode)Enum.Parse(typeof(KeyCode), key);

      if (type == EventType.Pressed)
        selector = () => Input.GetKey(keycode);
      else if (type == EventType.Down)
        selector = () => Input.GetKeyDown(keycode);
      else if (type == EventType.Up)
        selector = () => Input.GetKeyUp(keycode);

      return (T _) => selector();
    }
  }
}