using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// メインレンダラのビュー。最終出力の RawImage と各レイヤのビューへの参照を持ち、描画を行なう。
/// </summary>
public class MainRendererView : MonoBehaviour
{
    [SerializeField] private RawImage[] _mainImages;
    [SerializeField] private LayerView[] _layerViews;
    public IReadOnlyList<IObservable<float>> OnChangeBlendingValues { get; private set; }
    public IReadOnlyList<IObservable<float>> OnChangeSeekValues { get; private set; }
    public Layers SelectedLayer => _selectedLayer.Value;
    private readonly ReactiveProperty<Layers> _selectedLayer = new ReactiveProperty<Layers>(Layers.Layer1);

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="mainImageMaterial"></param>
    /// <param name="layerTextures"></param>
    public void Initialize(Material mainImageMaterial, IReadOnlyList<RenderTexture> layerTextures)
    {
        foreach (var m in _mainImages) m.material = mainImageMaterial;
        for (var i = 0; i < _layerViews.Length; i++)
        {
            _layerViews[i].Initialize(layerTextures[i], 0);
            var layer = Layers.Layer1 + i;
            _layerViews[i].OnClickButton.Subscribe(_ => _selectedLayer.Value = layer);
            var index = i;
            _selectedLayer.Subscribe(l => _layerViews[index].UpdateUI(isSelected: layer == l));
        }
        OnChangeBlendingValues = _layerViews.Select(v => v.OnChangeBlendingSliderValue).ToList();
        OnChangeSeekValues = _layerViews.Select(v => v.OnChangeSeekSliderValue).ToList();
    }

    /// <summary>
    /// BlendingFactor のスライダを操作する
    /// </summary>
    /// <param name="layer">対象のレイヤー</param>
    /// <param name="value">値</param>
    public void SetBlendingSlider(Layers layer, float value)
        => _layerViews[layer - Layers.Layer1].SetBlendingSliderValue(Mathf.Clamp01(value));

    /// <summary>
    /// Seek のスライダを操作する
    /// </summary>
    /// <param name="layer">対象のレイヤー</param>
    /// <param name="value">値</param>
    public void SetSeekSlider(Layers layer, float value)
        => _layerViews[layer - Layers.Layer1].SetSeekSliderValue(Mathf.Clamp01(value));

    public void UpdateLayerView(Layers layer, bool? isSelected = null, bool? showSeekBar = null, float? speed = null, float? attack = null)
        => _layerViews[layer - Layers.Layer1].UpdateUI(isSelected, showSeekBar, speed, attack);

    public void SetSelectedLayer(Layers layer) => _selectedLayer.Value = layer;
}