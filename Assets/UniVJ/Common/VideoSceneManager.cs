using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UniRx.Async;

public class VideoSceneManager : SubSceneManager
{
    [SerializeField] private VideoPlayer _frontVideo;
    [SerializeField] private VideoPlayer _backVideo;

    private double _length;
    private Action<float> _onUpdateTime;

    public async void LoadVideo(string url, Action<float> onUpdateTime)
    {
        _frontVideo.url = url;
        _backVideo.url = url;
        await UniTask.WhenAll(UniTask.WaitUntil(() => _frontVideo.isPrepared), UniTask.WaitUntil(() => _backVideo.isPrepared));
        _length = _frontVideo.length;
        _onUpdateTime = onUpdateTime;
    }

    public void SetSpeed(float speed)
    {
        _frontVideo.playbackSpeed = speed;
        _backVideo.playbackSpeed = speed;
    }

    public override void SetSeekValue(float value)
    {
        _frontVideo.time = _length * value;
        _backVideo.time = _length * value;
    }

    void Update()
    {
        _onUpdateTime?.Invoke((float)(_frontVideo.time / _length));
    }
}
