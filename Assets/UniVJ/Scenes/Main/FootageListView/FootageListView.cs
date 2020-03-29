using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace UniVJ
{
    /// <summary>
    /// 素材リスト
    /// </summary>
    public class FootageListView : MonoBehaviour
    {
        [SerializeField] private FootageScrollView _scrollView;
        public IObservable<FootageScrollViewData> OnSelectData => _scrollView.OnSelectData;

        public void Initialize(IList<FootageScrollViewData> items)
        {
            _scrollView.InitializeView();
            UpdateData(items);
            _scrollView.SelectCell(0);
        }

        public void UpdateData(IList<FootageScrollViewData> data)
        {
            _scrollView.UpdateData(data);
        }
    }
}