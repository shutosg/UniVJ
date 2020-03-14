using System;
using System.Collections.Generic;
using UnityEngine;
using EasingCore;
using FancyScrollView;

using UniRx;
using Zenject;

/// <summary>
/// 素材リスト
/// </summary>
public class FootageScrollView : FancyGridView<FootageScrollViewData, FootageScrollViewContext>
{

    class CellGroup : DefaultCellGroup { }

    [SerializeField] FootageScrollViewCell _cellPrefab;

    protected override void SetupCellTemplate() => Setup<CellGroup>(_cellPrefab);
    public IObservable<FootageScrollViewData> OnSelectData => _onSelectData;
    private Subject<FootageScrollViewData> _onSelectData = new Subject<FootageScrollViewData>();

    [Inject] private FootageManager _footageManager;
    [Inject] private ThumbnailMaker _thumbnailMaker;

    public void InitializeView() => Initialize();

    protected override void Initialize()
    {
        if(initialized) return;
        initialized = true;
        base.Initialize();
        // 選択されてる状態でタップされたら読み込む
        Context.OnCellClicked = i => {
            if(Context.SelectedIndex != i) SelectCell(i);
            else _onSelectData.OnNext(ItemsSource[i / startAxisCellCount][i % startAxisCellCount]);
        };
        Context.FootageManager = _footageManager;
        Context.ThumbnailMaker = _thumbnailMaker;
    }

    public void UpdateSelection(int index)
    {
        if (Context.SelectedIndex == index) return;

        Context.SelectedIndex = index;
        Refresh();
    }

    /// <summary>
    /// 与えられたデータでセルを更新する。ウィンドウのサイズに合わせてセルのサイズを更新する。
    /// </summary>
    /// <param name="items"></param>
    protected override void UpdateContents(IList<FootageScrollViewData[]> items)
    {
        var columnCount = startAxisCellCount;
        var width = (cellContainer as RectTransform).rect.width;
        // TODO: ScreenManager
        var targetWidth = width / columnCount - startAxisSpacing * (columnCount - 1) / columnCount;
        cellSize = new Vector2(targetWidth, targetWidth * cellSize.y / cellSize.x);
        base.UpdateContents(items);
        foreach(var group in pool)
        {
            var cells = group.transform.GetComponentsInChildren<FootageScrollViewCell>();
            foreach (var cell in cells)
            {
                (cell.transform as RectTransform).sizeDelta = cellSize;
            }
        }
    }

    public void UpdateData(IList<FootageScrollViewData> items) => UpdateContents(items);

    public void SelectCell(int index)
    {
        if (DataCount == 0 || !isValid(index)) return;

        UpdateSelection(index);
        ScrollTo(index, 0.2f, Ease.OutQuint);
    }

    private bool isValid(int index) => index >= 0 && index < DataCount;
}
