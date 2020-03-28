using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VolumeFloatParameterView : VolumeParameterView<float>
{
    [SerializeField] private Fader _fader;

    protected override void setValueView()
    {
        _valueView = _fader;
    }

    protected override void renderValueView(VolumeParameter<float> param)
    {
        if (param is ClampedFloatParameter clampedFloatParameter)
        {
            _fader.minValue = clampedFloatParameter.min;
            _fader.maxValue = clampedFloatParameter.max;
        }
        else
        {
            // 上限なしの場合とりあえず適当な範囲を設定
            _fader.minValue = -2 * Mathf.Max(Mathf.Abs(param.value), 2f);
            _fader.maxValue = 2 * Mathf.Max(Mathf.Abs(param.value), 2f);
        }
        _fader.SetValue(param.value);
    }
}