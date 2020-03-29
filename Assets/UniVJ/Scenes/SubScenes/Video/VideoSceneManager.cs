using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;
using UniRx.Async;

public class VideoSceneManager : SubSceneManager
{
    /// <summary>
    /// ある時点でのフレーム情報
    /// </summary>
    public class FrameInfo
    {
        public void Set(long frameNumber, float rate, double time)
        {
            Time = time;
            Rate = rate;
            FrameNumber = frameNumber;
        }

        /// <summary>フレーム</summary>
        public long FrameNumber { get; private set; }
        /// <summary>全尺に対する割合</summary>
        public float Rate { get; private set; }
        /// <summary>動画内時刻</summary>
        public double Time { get; private set; }
    }

    /// <summary>playbackSpeed の変更を反映するまで待機するフレーム数</summary>
    private const int SpeedChangeWaitFrame = 10;

    [SerializeField] private VideoPlayer _frontVideo;
    [SerializeField] private VideoPlayer _backVideo;

    /// <summary></summary>
    private readonly float[] _previousSpeeds = new float[SpeedChangeWaitFrame];
    private float _currentSpeed = 1f;
    private FrameInfo _info = new FrameInfo();
    public event Action<FrameInfo> OnUpdate;
    public double Length => _frontVideo.length;

    /// <summary>
    /// 読み込み
    /// </summary>
    /// <param name="url"></param>
    /// <param name="onUpdateTime">毎フレーム行なう処理(引数は現在の再生時刻)</param>
    /// <returns></returns>
    public async UniTask LoadVideo(string url, Action<FrameInfo> onUpdateTime = null)
    {
        _frontVideo.url = url;
        _backVideo.url = url;
        await UniTask.WaitUntil(() => _frontVideo.isPrepared && _backVideo.isPrepared);
        OnUpdate += onUpdateTime;
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
            _previousSpeeds[i] = v;
    }

    private bool isWaited()
    {
        if (_previousSpeeds == null) return false;
        foreach (var s in _previousSpeeds)
            if (!nearlyEquals(s, _currentSpeed))
                return false;

        return true;
    }

    private static bool nearlyEquals(float a, float b) => Math.Abs(a - b) <= 0.00001f;

    private void LateUpdate()
    {
        if (nearlyEquals(_frontVideo.playbackSpeed, _currentSpeed) || !isWaited()) return;
        _frontVideo.playbackSpeed = _currentSpeed;
        _backVideo.playbackSpeed = _currentSpeed;
    }

    public async UniTask FastForward(double seconds)
    {
        await setTime(_frontVideo.time + seconds);
    }

    public override async UniTask SetSeekValue(float value)
    {
        var isPaused = _frontVideo.isPaused;
        _frontVideo.Play();
        _backVideo.Play();
        await setTime(_frontVideo.length * value);
        if (isPaused) await Pause();
    }

    private async UniTask setTime(double targetTime)
    {
        _frontVideo.time = targetTime;
        _backVideo.time = targetTime;
        await UniTask.WaitUntil(() => _frontVideo.time >= targetTime && _backVideo.time >= targetTime);
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
        _info.Set(_frontVideo.frame, (float)(_frontVideo.time / _frontVideo.length), _frontVideo.time);
        OnUpdate?.Invoke(_info);
        setSpeedArray(_currentSpeed, Time.frameCount % SpeedChangeWaitFrame);
    }
}