using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using UniRx.Async;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private ControlPanel controlPanel;
    [SerializeField] private Renderer mainRenderer;

    void Start()
    {
        // ディスプレイ
        foreach(var d in Display.displays)
        {
            d.Activate();
        }

        mainRenderer.Initialize();
        controlPanel.Initialize(mainRenderer);
    }
}
