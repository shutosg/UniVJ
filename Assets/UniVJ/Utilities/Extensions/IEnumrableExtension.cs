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
    {
        Debug.Log($"{prefix}[{string.Join(", ", self.Select(obj => selector != null ? selector(obj) : obj.ToString()))}]{suffix}");
    }
}
