using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using UniRx.Async;

public class EntryPoint : MonoBehaviour
{
    public static readonly Vector2Int Resolution = new Vector2Int(1280, 720);
    [SerializeField] private ControlPanel _controlPanel;
    [SerializeField] private Shader _mainRendererShader;

    void Start()
    {
        // ディスプレイ
        foreach(var d in Display.displays)
        {
            d.Activate();
        }

        var mainRenderer = new MainRenderer();

        mainRenderer.Initialize(_mainRendererShader, Resolution);
        _controlPanel.Initialize(mainRenderer);
    }
}
