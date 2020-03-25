using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;
using UnityEngine.Video;
using UniRx.Async;
using UnityEngine.VFX;

public class ParticleWaveManager : SubSceneManager
{
    [SerializeField] private VisualEffect[] _vfxs;
    private int _frequencyId;
    private int _sizeId;
    private int _hueId;

    void Awake()
    {
        _frequencyId = Shader.PropertyToID("Frequency");
        _sizeId = Shader.PropertyToID("Size");
        _hueId = Shader.PropertyToID("Hue");
    }

    private void setFrequency(float value)
    {
        foreach (var vfx in _vfxs)
        {
            vfx.SetFloat(_frequencyId, value);
        }
    }

    private void setSpeed(float value)
    {
        foreach (var vfx in _vfxs)
        {
            vfx.SetFloat(_speedId, value);
        }
    }

    private void setHue(float value)
    {
        foreach (var vfx in _vfxs)
        {
            vfx.SetFloat(_hueId, value);
        }
    }

    private void setSize(float value)
    {
        foreach (var vfx in _vfxs)
        {
            vfx.SetFloat(_sizeId, value);
        }
    }

    protected override void onReceiveVariable1(float value) => setSize(value);
    protected override void onReceiveVariable2(float value) => setHue(value);
    protected override void onReceiveVariable3(float value) => setFrequency(value);

    public override void OnReceiveSpeed(float value) => setSpeed(value);
}