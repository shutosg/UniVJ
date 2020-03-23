using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;
using UnityEngine.Video;
using UniRx.Async;
using UnityEngine.VFX;

public class ParticleSpectrumManager : SubSceneManager
{
    [SerializeField] private VisualEffect[] _vfxs;
    [SerializeField] private MicSampler _sampler;
    private int _turbulencePowerId;

    void Awake()
    {
        _turbulencePowerId = Shader.PropertyToID("TurbulencePower");
        // Setup(null, Layers.Default);
    }

    public override void Setup(RenderTexture targetTexture, Layers layer)
    {
        _sampler.Initialize(null).Forget();
        base.Setup(targetTexture, layer);
    }

    protected override void onReceiveVariable1(float value)
    {
        foreach (var vfx in _vfxs)
        {
            vfx.SetFloat(_turbulencePowerId, Mathf.Lerp(-20, 20, value));
        }
    }

    public override void OnReceiveSpeed(float value)
    {
        foreach (var vfx in _vfxs)
        {
            vfx.SetFloat(_speedId, value);
        }
    }
}