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

    private Action<float> _onUpdateTime;

    public async UniTask LoadVideo(string url, Action<float> onUpdateTime = null)
    {
        _frontVideo.url = url;
        _backVideo.url = url;
        await UniTask.WaitUntil(() => _frontVideo.isPrepared && _backVideo.isPrepared);
        _onUpdateTime = onUpdateTime;
    }

    public void SetSpeed(float speed)
    {
        _frontVideo.playbackSpeed = speed;
        _backVideo.playbackSpeed = speed;
    }

    public override async UniTask SetSeekValue(float value)
    {
        var isPaused = _frontVideo.isPaused;
        _frontVideo.Play();
        _backVideo.Play();
        var targetTime = _frontVideo.length * value;
        _frontVideo.time = targetTime;
        _backVideo.time = targetTime;
        await UniTask.WaitUntil(() => _frontVideo.time >= targetTime && _backVideo.time >= targetTime);
        if(isPaused) await Pause();
    }

    public async UniTask Pause()
    {
        _frontVideo.Pause();
        _backVideo.Pause();
        await UniTask.WaitUntil(() => _frontVideo.isPaused && _backVideo.isPaused);
    }

    public override void OnReceiveSpeed(float value)
    {
        _frontVideo.playbackSpeed = value;
        _backVideo.playbackSpeed = value;
    }

    void Update()
    {
        _onUpdateTime?.Invoke((float)(_frontVideo.time / _frontVideo.length));
    }
}
