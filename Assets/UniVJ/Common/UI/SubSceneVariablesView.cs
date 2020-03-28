using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

public class SubSceneVariablesView : MonoBehaviour
{
    [SerializeField] private Fader[] _faders;
    public IReadOnlyList<IObservable<float>> OnValueChangeds => _faders.Select(f => f.OnValueChanged).ToList();

    public void SetValue(int index, float value)
    {
        if (index < 0 || index >= _faders.Length)
        {
            Debug.LogError($"指定されたindex {index} は _faders(Length: {_faders.Length}) のindex範囲外です");
            return;
        }
        _faders[index].SetValue(value);
    }
}