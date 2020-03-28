using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using UniRx.Async;
using UnityEngine.Serialization;

public class ParticleTunnelController : SubSceneController
{
    protected override bool initialize()
    {
        if (!tryCastSubSceneManager<ParticleTunnelManager>(out var particleTunnelManager)) return false;
        return true;
    }
}