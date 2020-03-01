using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        // TODO: フッテージマネージャがやる
        var footageData = Enumerable.Range(0, SceneManager.sceneCountInBuildSettings)
            .Select(i => SceneUtility.GetScenePathByBuildIndex(i))
            .Select(path => Path.GetFileNameWithoutExtension(path))
            .Where(name => name != "Main")
            .Select(name => new FootageScrollViewData(name))
            .ToList();
        _footageListView.Initialize(footageData);
        // レイヤビュー初期化
        var layerManager = new LayerManager(_mainRenderer);
        _footageListView.OnSelectData.Subscribe(data => layerManager.LoadSceneAsync(data.SceneName, _rendererView.SelectedLayer));
    }
}
