using System;
using System.Linq;
using UnityEngine;
using UniRx;

namespace InputRx
{
  public class TestInputConstraint : BaseInputConstraint
  {
    public BoolReactiveProperty Satisfaction = new BoolReactiveProperty();

    private void Start()
    {
      Satisfaction.Subscribe(_ => satisfactionProperty.Value = Satisfaction.Value);
    }
  }
}