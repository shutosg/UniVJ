using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DOValue : DOTweener
{
    [SerializeField] private float _startValue = 0;
    [SerializeField] private float _endValue = 1;

    public float CurrentValue { get; private set; }
    public Action<float> OnUpdate;

    protected override Tween playTween()
    {
        reset();
        return DOTween.To(() => CurrentValue, value =>
        {
            CurrentValue = value;
            OnUpdate?.Invoke(value);
        }, _endValue, _duration);
    }

    protected override void reset()
    {
        CurrentValue = _startValue;
    }
}