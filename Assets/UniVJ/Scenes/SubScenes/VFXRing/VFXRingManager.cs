using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;
using UnityEngine.Video;
using UniRx.Async;
using UnityEngine.VFX;

public class VFXRingManager : SubSceneManager
{
    [SerializeField] private VisualEffect _vfxRing;

    public override void OnReceiveAttack(float value)
    {
        _vfxRing.SetFloat("Radius", value * 10);
    }

    public override void OnReceiveSpeed(float value)
    {        
        _vfxRing.SetFloat("ParticleSize", value);
    }
}
