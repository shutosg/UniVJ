using System;
using System.Linq;
using UnityEngine;
using UniRx;
using Zenject;

/// <summary>
/// コントロールパネル。実質アプリケーションマネージャ。UIの初期化や操作を行なう。
/// </summary>
public class ControlPanel : MonoBehaviour
{
    [Inject] private MainRendererView _rendererView;
    [Inject] private FootageListView _footageListView;
    [Inject] private LayerManager _layerManager;
    [Inject] private FootageManager _footageManager;

    public void Initialize()
    {
        // 素材リスト初期化
        var footageData = _footageManager.GetAllFootageData().ToList();
        _footageListView.Initialize(footageData);
        // レイヤビュー初期化
        // リストのデータが選択されたら素材の読み込みと View の更新
        _footageListView.OnSelectData.Subscribe(async data =>
        {
            var selectedLayer = _rendererView.SelectedLayer;
            Action<float> onUpdateTime = null;
            var isVideo = data.Type == FootageType.Video;
            if (isVideo) onUpdateTime = value => _rendererView.SetSeekSlider(selectedLayer, value);
            await _layerManager.LoadFootage(data, selectedLayer, onUpdateTime);
            _rendererView.UpdateLayerView(selectedLayer, showSeekBar: isVideo, speed: 1f, attack: 0f);
            _rendererView.SetSeekSlider(selectedLayer, 0);
        });
    }

    public void SendAttack(float value)
    {
        _rendererView.UpdateLayerView(_rendererView.SelectedLayer, attack: value);
        _layerManager.SendAttack(_rendererView.SelectedLayer, value);
    }

    public void SendSpeed(Layers layer, float value)
    {
        var speed = Mathf.Lerp(0, 5, value);
        _rendererView.UpdateLayerView(layer, speed: speed);
        _layerManager.SendSpeed(layer, speed);
    }

    public void SetBlendingFactor(Layers layer, float value) => _rendererView.SetBlendingSlider(layer, value);
}