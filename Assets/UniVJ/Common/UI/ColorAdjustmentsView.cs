using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UniRx;

public class ColorAdjustmentsView : MonoBehaviour
{
    [SerializeField] private VolumeFloatParameterView _postExposure;
    [SerializeField] private VolumeFloatParameterView _contrast;
    [SerializeField] private VolumeColorParameterView _colorFilter;
    [SerializeField] private VolumeFloatParameterView _hueShift;
    [SerializeField] private VolumeFloatParameterView _saturation;

    public void Initialize(ColorAdjustments colorAdjustments)
    {
        _postExposure.Render(colorAdjustments.postExposure);
        _postExposure.OnChangeViewValue.Subscribe(value => setValue(colorAdjustments.postExposure, value));
        _postExposure.OnChangeViewToggle.Subscribe(isOn => setIsOn(colorAdjustments.postExposure, isOn));
        _contrast.Render(colorAdjustments.contrast);
        _contrast.OnChangeViewValue.Subscribe(value => setValue(colorAdjustments.contrast, value));
        _contrast.OnChangeViewToggle.Subscribe(isOn => setIsOn(colorAdjustments.contrast, isOn));
        _colorFilter.Render(colorAdjustments.colorFilter);
        _colorFilter.OnChangeViewValue.Subscribe(value => setValue(colorAdjustments.colorFilter, value));
        _colorFilter.OnChangeViewToggle.Subscribe(isOn => setIsOn(colorAdjustments.colorFilter, isOn));
        _hueShift.Render(colorAdjustments.hueShift);
        _hueShift.OnChangeViewValue.Subscribe(value => setValue(colorAdjustments.hueShift, value));
        _hueShift.OnChangeViewToggle.Subscribe(isOn => setIsOn(colorAdjustments.hueShift, isOn));
        _saturation.Render(colorAdjustments.saturation);
        _saturation.OnChangeViewValue.Subscribe(value => setValue(colorAdjustments.saturation, value));
        _saturation.OnChangeViewToggle.Subscribe(isOn => setIsOn(colorAdjustments.saturation, isOn));
    }

    private void setValue<T>(VolumeParameter<T> param, T value)
    {
        param.overrideState = true;
        param.value = value;
    }

    private void setIsOn(VolumeParameter param, bool isOn) => param.overrideState = isOn;
}