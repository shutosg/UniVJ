using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ImageSceneController : SubSceneController
{
    [SerializeField] private Toggle _backgroundToggle;
    private ImageSceneManager _imageSceneManager;

    protected override bool initialize()
    {
        if (!tryCastSubSceneManager<ImageSceneManager>(out _imageSceneManager)) return false;
        _backgroundToggle.OnValueChangedAsObservable().Subscribe(isOn =>
        {
            var color = isOn ? Color.white : Color.black;
            _imageSceneManager.SetBackground(isOn, color);
        });
        return true;
    }
}