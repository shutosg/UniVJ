using System.Linq;
using UnityEngine;
using UniRx;

/// <summary>
/// コントロールパネル。実質アプリケーションマネージャ。UIの初期化や操作を行なう。
/// </summary>
public class ControlPanel : MonoBehaviour
{
    [SerializeField] private MainRendererView _rendererView;
    [SerializeField] private FootageListView _footageListView;
    private MainRenderer _mainRenderer;

    public void Initialize(MainRenderer mainRenderer)
    {
        _mainRenderer = mainRenderer;
        _mainRenderer.InitializeView(_rendererView);
        // 素材リスト初期化
        var footageData = FootageManager.GetAllFootageData().ToList();
        _footageListView.Initialize(footageData);
        // レイヤビュー初期化
        var layerManager = new LayerManager(_mainRenderer);
        _footageListView.OnSelectData.Subscribe(data => layerManager.LoadFootage(data, _rendererView.SelectedLayer));
    }
}
