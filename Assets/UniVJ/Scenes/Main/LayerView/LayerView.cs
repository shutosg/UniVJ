using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

/// <summary>
/// レイヤーのプレビューパネル
/// </summary>
public class LayerView : MonoBehaviour
{
    [SerializeField] private RawImage _cursor;
    [SerializeField] private Button _selectButton;
    [SerializeField] private RawImage _mainImage;
    [SerializeField] private Slider _seekBar;
    [SerializeField] private Slider _blendingFactor;
    [SerializeField] private TextMeshProUGUI _speed;
    [SerializeField] private TextMeshProUGUI _attack;

    public IObservable<float> OnChangeBlendingSliderValue => _blendingFactor.OnValueChangedAsObservable();
    public IObservable<float> OnChangeSeekSliderValue => _seekBar.OnValueChangedAsObservable();
    public IObservable<Unit> OnClickButton => _selectButton.OnClickAsObservable();

    public void Initialize(RenderTexture renderTexture, float blendingFactor)
    {
        _mainImage.texture = renderTexture;
        _blendingFactor.value = blendingFactor;
        _seekBar.gameObject.SetActive(false);
    }

    public void SetBlendingSliderValue(float value) => _blendingFactor.value = value;
    public void SetSeekSliderValue(float value) => _seekBar.value = value;

    public void UpdateUI(bool? isSelected = null, bool? showSeekBar = null, float? speed = null, float? attack = null)
    {
        if(isSelected != null)
            _cursor.color = isSelected.Value ? Color.red : Color.gray;

        if(showSeekBar != null)
            _seekBar.gameObject.SetActive(showSeekBar.Value);

        if (speed != null)
            _speed.SetText($"Speed: {speed.Value:F4}");

        if(attack != null)
            _attack.SetText($"Speed: {attack.Value:F4}");

    }
}
