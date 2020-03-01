using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

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
    public IObservable<float> OnChangeBlendingSliderValue => _blendingFactor.OnValueChangedAsObservable();
    public IObservable<Unit> OnClickButton => _selectButton.OnClickAsObservable();

    public void Initialize(RenderTexture renderTexture, float blendingFactor)
    {
        _mainImage.texture = renderTexture;
        _blendingFactor.value = blendingFactor;
        _seekBar.gameObject.SetActive(false);
    }

    public void SetBlendingSliderValue(float value) => _blendingFactor.value = value;

    public void UpdateUI(bool isSelected)
    {
        _cursor.color = isSelected ? Color.red : Color.gray;
    }
}
