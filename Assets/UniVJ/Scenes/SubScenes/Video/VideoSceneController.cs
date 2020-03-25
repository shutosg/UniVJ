using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using UniRx.Async;
using UnityEngine.Serialization;

public class VideoSceneController : SubSceneController
{
    [SerializeField] private Button _rewind10SecondsButton;
    [SerializeField] private Button _fastForward10SecondsButton;
    [SerializeField] private Button _set0PercentButton;
    [SerializeField] private Button _set25PercentButton;
    [SerializeField] private Button _set50PercentButton;
    [SerializeField] private Button _set75PercentButton;
    [SerializeField] private TextMeshProUGUI _length;
    [SerializeField] private TextMeshProUGUI _currentTime;

    protected override bool initialize()
    {
        if (!tryCastSubSceneManager<VideoSceneManager>(out var videoSceneManager)) return false;
        _rewind10SecondsButton.OnClickAsObservable().Subscribe(_ => videoSceneManager.FastForward(-10f).Forget());
        _fastForward10SecondsButton.OnClickAsObservable().Subscribe(_ => videoSceneManager.FastForward(10f).Forget());
        _set0PercentButton.OnClickAsObservable().Subscribe(_ => videoSceneManager.SetSeekValue(0).Forget());
        _set25PercentButton.OnClickAsObservable().Subscribe(_ => videoSceneManager.SetSeekValue(0.25f).Forget());
        _set50PercentButton.OnClickAsObservable().Subscribe(_ => videoSceneManager.SetSeekValue(0.5f).Forget());
        _set75PercentButton.OnClickAsObservable().Subscribe(_ => videoSceneManager.SetSeekValue(0.75f).Forget());
        _length.text = $"[Length] {TimeSpan.FromSeconds((int)videoSceneManager.Length):c}";
        videoSceneManager.OnUpdate += info =>
        {
            var ts = TimeSpan.FromSeconds(info.Time);
            _currentTime.SetText("[Current] {0}:{1}:{2:2}", (int)ts.Hours, ts.Minutes, ts.Seconds + ts.Milliseconds / 1000f);
        };
        return true;
    }
}