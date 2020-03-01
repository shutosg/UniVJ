using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using UniRx.Async;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private ControlPanel _controlPanel;
    [SerializeField] private Shader _mainRendererShader;

    void Start()
    {
        // ディスプレイ
        foreach(var d in Display.displays)
        {
            d.Activate();
        }

        var mainRenderer = new Renderer();

        mainRenderer.Initialize(_mainRendererShader);
        _controlPanel.Initialize(mainRenderer);
    }
}
