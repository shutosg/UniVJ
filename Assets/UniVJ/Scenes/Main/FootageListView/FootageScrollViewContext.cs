using System;
using FancyScrollView;

namespace UniVJ
{
    /// <summary>
    /// 素材リストのコンテキスト
    /// </summary>
    public class FootageScrollViewContext : FancyGridViewContext
    {
        public int SelectedIndex = -1;
        public Action<int> OnCellClicked;
        public FootageManager FootageManager;
        public ThumbnailMaker ThumbnailMaker;
    }
}