using System;
using System.Linq;
using System.Collections.Generic;
using UniRx;

public static class LinqHelper
{
  public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
  {
    foreach (T item in enumerable)
      action(item);
  }

  public static IEnumerable<T> WhereEquals<T>(this IEnumerable<T> collection, T value)
  {
    return collection.Where(item => item.Equals(value));
  }

  public static IObservable<T> WhereEquals<T>(this IObservable<T> collection, T value)
  {
    return collection.Where(item => item.Equals(value));
  }
}