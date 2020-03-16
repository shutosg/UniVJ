using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using FancyScrollView;
using UniRx.Async;

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
    private ThumbnailMaker _thumbnailMaker => Context.ThumbnailMaker;

    void Start()
    {
        _button.onClick.AddListener(() => Context.OnCellClicked?.Invoke(Index));
        _loadButton.onClick.AddListener(() => Context.OnCellClicked?.Invoke(Index));
    }

    public override void UpdateContent(FootageScrollViewData itemData)
    {
        // 表示名
        _name.text = itemData.DisplayName;
        // 選択状態
        var selected = Context.SelectedIndex == Index;
        _cursorImage.color = selected ? _selectedCursorColor : _nonSelectedCursorColor;
        // サムネイル
        setupThumbnail(itemData);
    }

    private void setupThumbnail(FootageScrollViewData data)
    {
        // 一旦リセット
        _screenShot.texture = null;
        _screenShot.color = Color.white;
        // TODO: ScreenManager
        _aspectRatioFitter.aspectRatio = 192f / 108;

        // サムネイル読み込み
        cancellationTokenSource?.Cancel();
        cancellationTokenSource = new CancellationTokenSource();
        switch (data.Type)
        {
            case FootageType.Scene:
                _screenShot.color = new Color(0.8f, 0.25f, 1.0f, 1.0f);
                break;
            case FootageType.Video:
                // サムネイルがあれば読み込む、なければ作る
                var path = data.FootagePath;
                if (_thumbnailMaker.HasThumbnail(path))
                {
                    load(_thumbnailMaker.GetThumbnailPath(data.FootagePath).Remove(0, FootageManager.FootagePath.Length),
                        cancellationTokenSource.Token).Forget();
                }
                else
                {
                    _screenShot.color = Color.cyan;
                    _thumbnailMaker.EnqueueMakeThumbnail(path, onLoaded, cancellationTokenSource.Token);
                }
                break;
            case FootageType.Image:
                load(data.DisplayName, cancellationTokenSource.Token).Forget();
                break;
        }

        // サムネイルを読み込む
        async UniTask load(string fileName, CancellationToken token)
        {
            try
            {
                var tex = await Context.FootageManager.LoadTexture(fileName, token);
                onLoaded(tex);
            }
            catch (OperationCanceledException) { }
        }

        // サムネイルの読み込み完了後処理
        void onLoaded(Texture2D tex)
        {
            _screenShot.texture = tex;
            _screenShot.color = Color.white;
            _aspectRatioFitter.aspectRatio = (float)tex.width / tex.height;
        }
    }
}