using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Async;

public class MicSampler : MonoBehaviour
{
    [SerializeField] AudioSource _source;
    [SerializeField, Range(0f, 10f)] float _gain = 1f; // 音量に掛ける倍率

    public bool IsInitialized { get; private set; }
    private float[] _spectrum;

    public async UniTask Initialize(float[] spectrum)
    {
        if ((Microphone.devices.Length == 0))
        {
            Debug.LogError("マイクが認識されていません");
            return;
        }

        _spectrum = spectrum;
        _source.loop = true;
        _source.clip = Microphone.Start(null, true, 10, 44100);
        // マイクの準備が整うまで待つ
        await UniTask.WaitUntil(() => Microphone.GetPosition(null) > 0);
        _source.Play();
        IsInitialized = true;
    }

    public void UpdateValues()
    {
        if (!IsInitialized) return;
        _source.GetSpectrumData(_spectrum, 0, FFTWindow.Hamming);
        for (var i = 0; i < _spectrum.Length; i++)
        {
            _spectrum[i] *= _gain;
        }
    }
}