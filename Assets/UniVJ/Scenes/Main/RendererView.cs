using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RendererView : MonoBehaviour
{
    [SerializeField] private RawImage _mainImage;
    [SerializeField] private LayerView[] _layerViews;
    public IReadOnlyList<IObservable<float>> OnChangeBlendingValues { get; private set; }

    public void Initialize(Material mainImageMaterial, IReadOnlyList<RenderTexture> layerTextures)
    {
        _mainImage.material = mainImageMaterial;
        for (var i = 0; i < _layerViews.Length; i++)
        {
            _layerViews[i].Initialize(layerTextures[i], 0);
        }
        OnChangeBlendingValues = _layerViews.Select(v => v.OnChangeBlendingSliderValue).ToList();
    }

    public void SetBlendingSlider(int index, float value) => _layerViews[index].SetBlendingSliderValue(value);
}
