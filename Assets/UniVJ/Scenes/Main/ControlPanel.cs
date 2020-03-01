using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Async;

using UnityEngine.SceneManagement;

public class ControlPanel : MonoBehaviour
{
    [SerializeField] private Button _scene1;
    [SerializeField] private Button _scene2;
    [SerializeField] private RendererView _rendererView;
    private Renderer _mainRenderer;

    public void Initialize(Renderer mainRenderer)
    {
        _mainRenderer = mainRenderer;
        _mainRenderer.InitializeView(_rendererView);
        _scene1.OnClickAsObservable().Subscribe(_ => loadScene("Test1", 0));
        _scene2.OnClickAsObservable().Subscribe(_ => loadScene("Test2", 1));
    }

    private async void loadScene(string sceneName, int layerIndex)
    {
        await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        var subSceneCamera = SceneManager.GetSceneByName(sceneName).GetRootGameObjects()[0].GetComponentInChildren<SubSceneCamera>();
        _mainRenderer.RegistorySubScene(subSceneCamera, layerIndex);
    }
}
