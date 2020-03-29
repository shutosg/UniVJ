using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DORotater : DOTweener
{
    [SerializeField] private Vector3 _startValue;
    [SerializeField] private Vector3 _endValue;

    protected override Tween playTween()
    {
        reset();
        return transform.DORotate(_endValue, _duration, RotateMode.FastBeyond360);
    }

    protected override void reset()
    {
        transform.localEulerAngles = _startValue;
    }
}