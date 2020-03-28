using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VolumeColorParameterView : VolumeParameterView<Color>
{
    [SerializeField] private ColorSelector _colorSelector;

    protected override void setValueView()
    {
        _valueView = _colorSelector;
    }
}