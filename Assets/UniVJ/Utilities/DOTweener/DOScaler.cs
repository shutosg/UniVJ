using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DOScaler : DOTweener
{
    [SerializeField] private float _startValue = 1;
    [SerializeField] private float _endValue = 1;

    protected override Tween playTween()
    {
        reset();
        return transform.DOScale(_endValue, _duration);
    }

    protected override void reset()
    {
        transform.SetLocalScale(_startValue);
    }
}