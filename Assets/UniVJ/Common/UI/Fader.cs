using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UniRx;

public class Fader : VolumeParameterValueView<float>
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _valueText;
    public override IObservable<float> OnValueChanged => _slider.OnValueChangedAsObservable();

    public float minValue
    {
        get => _slider.minValue;
        set => _slider.minValue = value;
    }

    public float maxValue
    {
        get => _slider.maxValue;
        set => _slider.maxValue = value;
    }

    public override void SetValue(float value)
    {
        _slider.value = value;
        setText(value);
    }

    private void Awake()
    {
        _slider.onValueChanged.AddListener(setText);
    }

    private void setText(float value) => _valueText.SetText("{0:4}", value);
}