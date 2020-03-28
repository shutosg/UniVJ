using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UniRx;

public class ColorSelector : VolumeParameterValueView<Color>
{
    [SerializeField] private ColorPicker _colorPicker;
    [SerializeField] private Button _selectButton;
    [SerializeField] private Button _decideButton;
    private Color _cachedColor;
    public override IObservable<Color> OnValueChanged => _onValueChanged;
    private readonly Subject<Color> _onValueChanged = new Subject<Color>();

    public override void SetValue(Color value)
    {
        _colorPicker.CurrentColor = value;
        setColor(value);
        _colorPicker.onValueChanged.AddListener(c => _onValueChanged.OnNext(c));
    }

    private void Awake()
    {
        _colorPicker.onValueChanged.AddListener(setColor);
        _selectButton.OnClickAsObservable().Subscribe(_ => _colorPicker.gameObject.SetActive(true));
        _decideButton.OnClickAsObservable().Subscribe(_ => _colorPicker.gameObject.SetActive(false));
    }

    /// <summary>
    /// Esc. or Return で非表示
    /// </summary>
    private void Update()
    {
        if (!_colorPicker.gameObject.activeInHierarchy) return;
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return)) _colorPicker.gameObject.SetActive(false);
    }

    private void setColor(Color value) => _selectButton.image.color = value;
}