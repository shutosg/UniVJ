using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class IEnumerableExtension
{
    /// <summary>
    /// IEnumerable を読める形式で Debug.Log() する
    /// </summary>
    /// <param name="self"></param>
    /// <param name="prefix">IEnumerable の前に付け足す文字列</param>
    /// <param name="suffix">IEnumerable の後に付け足す文字列</param>
    /// <param name="selector">IEnumerable の要素を加工する関数</param>
    /// <typeparam name="T"></typeparam>
    public static void DebugLog<T>(this IEnumerable<T> self, string prefix = null, string suffix = null, Func<T, string> selector = null)
        => Debug.Log(self.ToDebugString(prefix, suffix, selector));


    public static string ToDebugString<T>(this IEnumerable<T> self, string prefix = null, string suffix = null, Func<T, string> selector = null)
        => $"{prefix}[{string.Join(", ", self.Select(obj => selector != null ? selector(obj) : obj.ToString()))}]{suffix}";

    /// <summary>
    /// 2つのリストから1つづつ取り出して処理を実行する(index付き)
    /// </summary>
    /// <param name="self"></param>
    /// <param name="second"></param>
    /// <param name="action"></param>
    /// <typeparam name="First"></typeparam>
    /// <typeparam name="Second"></typeparam>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static void Zip<First, Second>(this IReadOnlyList<First> self, IReadOnlyList<Second> second, Action<First, Second, int> action)
    {
        if (second == null) throw new ArgumentNullException();
        if (self.Count() != second.Count()) throw new ArgumentException();
        for (var i = 0; i < self.Count(); i++)
            action?.Invoke(self[i], second[i], i);
    }

    /// <summary>
    /// 2つのリストから1つづつ取り出して処理を実行する
    /// </summary>
    /// <param name="self"></param>
    /// <param name="second"></param>
    /// <param name="action"></param>
    /// <typeparam name="First"></typeparam>
    /// <typeparam name="Second"></typeparam>
    public static void Zip<First, Second>(this IReadOnlyList<First> self, IReadOnlyList<Second> second, Action<First, Second> action)
        => Zip(self, second, (f, s, _) => action(f, s));

    /// <summary>
    /// 要素を1つづつ取り出して処理を実行する
    /// </summary>
    /// <param name="self"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    public static void ForEach<T>(this IReadOnlyList<T> self, Action<T, int> action)
    {
        for (var i = 0; i < self.Count(); i++) action?.Invoke(self[i], i);
    }

    /// <summary>
    /// 要素を1つづつ取り出して処理を実行する
    /// </summary>
    /// <param name="self"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    public static void ForEach<T>(this T[] self, Action<T> action)
    {
        for (var i = 0; i < self.Length; i++) action?.Invoke(self[i]);
    }
}