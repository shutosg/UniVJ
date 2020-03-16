using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UniRx.Async;

public class VideoSceneManager : SubSceneManager
{
    /// <summary>playbackSpeed の変更を反映するまで待機するフレーム数</summary>
    private const int SpeedChangeWaitFrame = 10;
    [SerializeField] private VideoPlayer _frontVideo;
    [SerializeField] private VideoPlayer _backVideo;

    private float[] _previousSpeeds;
    private float _currentSpeed;

    private Action<float> _onUpdateTime;

    public async UniTask LoadVideo(string url, Action<float> onUpdateTime = null)
    {
        _frontVideo.url = url;
        _backVideo.url = url;
        await UniTask.WaitUntil(() => _frontVideo.isPrepared && _backVideo.isPrepared);
        _onUpdateTime = onUpdateTime;
        _previousSpeeds = new float[SpeedChangeWaitFrame];
        setSpeedArray(1f);
        _currentSpeed = 1f;
    }

    private void setSpeedArray(float v, int? index = null)
    {
        if (_previousSpeeds == null) return;
        if (index != null)
        {
            _previousSpeeds[index.Value] = v;
            return;
        }
        for (var i = 0; i < _previousSpeeds.Length; i++)
        {
            _previousSpeeds[i] = v;
        }
    }

    private bool isWaited()
    {
        if (_previousSpeeds == null) return false;
        foreach (var s in _previousSpeeds)
        {
            if(!isSameValue(s, _currentSpeed)) return false;
        }
        return true;
    }

    private static bool isSameValue(float a, float b) => Math.Abs(a - b) <= 0.00001f;
    
    private void LateUpdate()
    {
        if (isSameValue(_frontVideo.playbackSpeed, _currentSpeed) || !isWaited()) return;
        _frontVideo.playbackSpeed = _currentSpeed;
        _backVideo.playbackSpeed = _currentSpeed;
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
        // playbackSpeed を急に変えるとガタガタするので、数フレーム待ってから変える
        _currentSpeed = value;
    }

    void Update()
    {
        _onUpdateTime?.Invoke((float)(_frontVideo.time / _frontVideo.length));
        setSpeedArray(_currentSpeed, Time.frameCount % SpeedChangeWaitFrame);
    }
}
