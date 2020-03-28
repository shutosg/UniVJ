using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UniRx;

public abstract class VolumeParameterView<T> : MonoBehaviour
{
    protected VolumeParameterValueView<T> _valueView;
    [SerializeField] protected Toggle _isOverride;
    public IObservable<T> OnChangeViewValue => _valueView.OnValueChanged;
    public IObservable<bool> OnChangeViewToggle => _isOverride.OnValueChangedAsObservable();
    private bool _initialized;

    protected void initializeIfNeed()
    {
        if (_initialized) return;
        _initialized = true;
        setValueView();
        _valueView.OnValueChanged.Subscribe(value =>
        {
            _isOverride.isOn = true;
        });
    }

    protected abstract void setValueView();

    public void Render(VolumeParameter<T> param)
    {
        initializeIfNeed();
        _isOverride.isOn = param.overrideState;
        renderValueView(param);
    }

    protected virtual void renderValueView(VolumeParameter<T> param)
    {
        _valueView.SetValue(param.value);
    }
}