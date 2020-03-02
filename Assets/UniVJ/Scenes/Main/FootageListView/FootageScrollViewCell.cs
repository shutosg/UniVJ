using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using FancyScrollView;

/// <summary>
/// 素材リストのセル。
/// </summary>
public class FootageScrollViewCell : FancyGridViewCell<FootageScrollViewData, FootageScrollViewContext>
{
    [SerializeField] Text _name;
    [SerializeField] Image _cursorImage;
    [SerializeField] RawImage _screenShot;
    [SerializeField] AspectRatioFitter _aspectRatioFitter;
    [SerializeField] Button _loadButton;
    [SerializeField] Button _button;
    [SerializeField] Color _selectedCursorColor;
    [SerializeField] Color _nonSelectedCursorColor;
    private CancellationTokenSource cancellationTokenSource;

    void Start()
    {
        _button.onClick.AddListener(() => Context.OnCellClicked?.Invoke(Index));
        _loadButton.onClick.AddListener(() => {
            Context.OnCellClicked?.Invoke(Index);
            Context.OnCellSelectClicked?.Invoke(Index);
        });
    }

    public override void UpdateContent(FootageScrollViewData itemData)
    {
        _name.text = itemData.DisplayName;

        _screenShot.texture = null;
        _aspectRatioFitter.aspectRatio = 192f / 108;

        cancellationTokenSource?.Cancel();
        cancellationTokenSource = new CancellationTokenSource();
        switch(itemData.Type)
        {
            case FootageType.Scene:
                break;
            case FootageType.Video:
                break;
            case FootageType.Image:
                loadTexture(cancellationTokenSource.Token);
                break;
        }

        var selected = Context.SelectedIndex == Index;
        _cursorImage.color = selected ? _selectedCursorColor : _nonSelectedCursorColor;

        async void loadTexture(CancellationToken token)
        {
            try {
                var tex = await FootageManager.LoadTexture(itemData.DisplayName, token);
                _screenShot.texture = tex;
                _aspectRatioFitter.aspectRatio = (float)tex.width / tex.height;
            } catch (OperationCanceledException) { }
        }
    }
}

