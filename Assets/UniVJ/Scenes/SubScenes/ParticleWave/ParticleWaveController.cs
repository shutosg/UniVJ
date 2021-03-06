﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using UniRx.Async;
using UnityEngine.Serialization;

public class ParticleWaveController : SubSceneController
{
    protected override bool initialize()
    {
        if (!tryCastSubSceneManager<ParticleWaveManager>(out var particleWaveManager)) return false;
        return true;
    }
}