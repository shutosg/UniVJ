using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx.Async;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private ControlPanel _controlPanel;

    void Start()
    {
        // ディスプレイ
        foreach (var d in Display.displays)
        {
            d.Activate();
        }
        _controlPanel.Initialize();
    }
}