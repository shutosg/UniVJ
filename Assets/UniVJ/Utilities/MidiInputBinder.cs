using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MidiJack;
using UnityEngine;

public class MidiInputBinder : MonoBehaviour
{
    private const float Tolerance = 0.0000001f;
    private readonly List<int> _knobNumbers = new List<int>();
    private readonly Dictionary<int, float> _previousValues = new Dictionary<int, float>();

    private bool _isInitialized;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="knobNumbers">リッスンするMIDIコントローラーのknobNumber</param>
    public void Initialize(int[] knobNumbers)
    {
        // 受け取った knobNumber 配列を元に _previousValues 初期化
        _knobNumbers.AddRange(knobNumbers);
        _knobNumbers.ForEach(knobNumber => _previousValues[knobNumber] = 0);
        _isInitialized = true;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        if (!_isInitialized) return;
        checkValues(_knobNumbers);
    }

    /// <summary>
    /// 対象の knobNumber の値に変化があればイベントを発火する
    /// </summary>
    /// <param name="targetNumbers">調べるknobNumberの配列</param>
    private void checkValues(List<int> targetNumbers)
    {
        foreach (var knobNumber in targetNumbers)
        {
            var value = MidiMaster.GetKnob(knobNumber);
            if (!(Math.Abs(_previousValues[knobNumber] - value) > Tolerance)) continue;
            _previousValues[knobNumber] = value;
            Debug.Log(knobNumber + ": " + value);
        }
    }
}