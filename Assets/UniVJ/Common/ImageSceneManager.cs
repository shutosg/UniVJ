using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Async;

public class ImageSceneManager : SubSceneManager
{
    [SerializeField] private RawImage _frontImage;
    [SerializeField] private RawImage _backImage;
    [SerializeField] private AspectRatioFitter[] aspectFitters;

    public async void LoadImage(string fileName)
    {
        var tex = await FootageManager.LoadTexture(fileName);
        _frontImage.texture = tex;
        _backImage.texture = tex;
        foreach (var fitter in aspectFitters)
            fitter.aspectRatio = (float)tex.width / tex.height;
    }
}
