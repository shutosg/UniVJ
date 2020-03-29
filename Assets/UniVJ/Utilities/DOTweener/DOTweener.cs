using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class DOTweener : MonoBehaviour
{
    [SerializeField] private bool _playOnStart = true;
    [SerializeField] private bool _isLoop;
    [SerializeField] private int _loopNum = -1;
    [SerializeField] private LoopType _loopType = LoopType.Yoyo;
    [SerializeField] private Ease _ease = Ease.Linear;
    [SerializeField] protected float _duration = 1;

    private Tween _playingTween;

    void Start()
    {
        if (!_playOnStart) return;
        Play();
    }

    /// <summary>
    /// 共通項目を設定して再生
    /// </summary>
    public void Play()
    {
        if (_playingTween != null) Stop();
        _playingTween = playTween();
        // ループ
        if (_isLoop)
        {
            _playingTween.SetLoops(_loopNum, _loopType);
        }
        // イージング
        if (_ease != Ease.Unset)
        {
            _playingTween.SetEase(_ease);
        }
        _playingTween.onComplete += () => Stop();
    }

    public void Stop(bool resetValue = false)
    {
        if (_playingTween != null)
        {
            _playingTween.Kill();
            _playingTween = null;
        }
        if (resetValue) reset();
    }

    /// <summary>
    /// 初期値に戻す
    /// </summary>
    public void Reset()
    {
        Stop();
        reset();
    }

    public void SetSpeed(float speed)
    {
        if (_playingTween == null) return;
        _playingTween.timeScale = speed;
    }

    protected abstract Tween playTween();

    protected abstract void reset();
}