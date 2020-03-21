using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx.Async;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private ControlPanel _controlPanel;
    [SerializeField] private MidiInputBinder _binder;

    void Start()
    {
        // ディスプレイ
        foreach (var d in Display.displays)
        {
            d.Activate();
        }
        _controlPanel.Initialize();

        // nano KONTROL2 で使う knobNumber
        _binder.Initialize(new[]
        {
            0, 1, 2, 3, 4, 5, 6, 7,
            16, 17, 18, 19, 20, 21, 22, 23,
            32, 33, 34, 35, 36, 37, 38, 39,
            41, 42, 43, 44, 45, 46,
            48, 49, 50, 51, 52, 53, 54, 55, 58, 59, 60, 61, 62,
            64, 65, 66, 67, 68, 69, 71
        });
    }
}