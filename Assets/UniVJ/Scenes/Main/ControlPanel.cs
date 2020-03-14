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
    [Inject] MainRendererView _rendererView;
    [Inject] FootageListView _footageListView;
    [Inject] LayerManager _layerManager;
    [Inject] FootageManager _footageManager;

    public void Initialize()
    {
        // 素材リスト初期化
        var footageData = _footageManager.GetAllFootageData().ToList();
        _footageListView.Initialize(footageData);
        // レイヤビュー初期化
        // リストのデータが選択されたら素材の読み込みと View の更新
        _footageListView.OnSelectData.Subscribe(async data => {
            var selectedLayer = _rendererView.SelectedLayer;
            Action<float> onUpdateTime = null;
            var isVideo = data.Type == FootageType.Video;
            if (isVideo) onUpdateTime = value => _rendererView.SetSeekSlider(selectedLayer, value);
            await _layerManager.LoadFootage(data, selectedLayer, onUpdateTime);
            _rendererView.UpdateLayerView(selectedLayer, showSeekBar: isVideo);
            _rendererView.SetSeekSlider(selectedLayer, 0);
        });
    }
}
