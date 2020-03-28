﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using UniRx.Async;
using UnityEngine.Serialization;

public class ImageSceneController : SubSceneController
{
    protected override bool initialize()
    {
        if (!tryCastSubSceneManager<ImageSceneManager>(out var imageSceneManager)) return false;
        return true;
    }
}