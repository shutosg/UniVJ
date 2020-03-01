using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ControlPanel : MonoBehaviour
{
    [SerializeField] private Button _scene1;
    [SerializeField] private Button _scene2;
    [SerializeField] private MainRendererView _rendererView;
    private MainRenderer _mainRenderer;

    public void Initialize(MainRenderer mainRenderer)
    {
        _mainRenderer = mainRenderer;
        _mainRenderer.InitializeView(_rendererView);
        var layerManager = new LayerManager(_mainRenderer);
        _scene1.OnClickAsObservable().Subscribe(_ => layerManager.LoadSceneAsync("Test1", _rendererView.SelectedLayer));
        _scene2.OnClickAsObservable().Subscribe(_ => layerManager.LoadSceneAsync("Test2", _rendererView.SelectedLayer));
    }
}
