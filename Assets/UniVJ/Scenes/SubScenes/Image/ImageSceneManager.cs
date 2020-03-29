using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx.Async;
using DG.Tweening;

public class ImageSceneManager : SubSceneManager
{
    [SerializeField] private RawImage _frontImage;
    [SerializeField] private RawImage _backImage;
    [SerializeField] private DOTweener[] _tweeners;
    [SerializeField] private AspectRatioFitter[] aspectFitters;

    [Inject] private FootageManager _footageManager;

    private Tweener _frontTween;

    public async UniTask LoadImage(string fileName)
    {
        var tex = await _footageManager.LoadTexture(fileName);
        _frontImage.texture = tex;
        _backImage.texture = tex;
        foreach (var fitter in aspectFitters)
            fitter.aspectRatio = (float)tex.width / tex.height;
        foreach (var tweener in _tweeners)
        {
            tweener.Play();
        }
    }

    public void SetBackground(bool fillWithImage, Color? fillColor = null)
    {
        _backImage.texture = fillWithImage ? _frontImage.texture : null;
        _backImage.color = fillColor ?? Color.white;
    }

    public override void OnReceiveSpeed(float value) => setTweenSpeed(value);

    protected override void onReceiveVariable1(float value) => setTweenSpeed(value);

    protected void setTweenSpeed(float value)
    {
        foreach (var tweener in _tweeners)
        {
            tweener.SetSpeed(value);
        }
    }
}