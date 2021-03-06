﻿using System;
using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Async;
using Zenject;

namespace UniVJ
{
    /// <summary>
    /// コントロールパネル。実質アプリケーションマネージャ。UIの初期化や操作を行なう。
    /// </summary>
    public class ControlPanel : MonoBehaviour
    {
        [SerializeField] private Transform _subSceneControllerParent;
        [SerializeField] private DOValue _flashTween;
        [Inject] private MainRendererView _rendererView;
        [Inject] private FootageListView _footageListView;
        [Inject] private LayerManager _layerManager;
        [Inject] private FootageManager _footageManager;
        [Inject] private IKeyInputBinder _keyBinder;

        [SerializeField] private VideoSceneController prefab;

        private Layers _flashingLayer = Layers.Layer1;

        public void Initialize()
        {
            // 素材リスト初期化
            var footageData = _footageManager.GetAllFootageData().ToList();
            _footageListView.Initialize(footageData);
            // レイヤビュー初期化
            // リストのデータが選択されたら素材の読み込みと View の更新
            _footageListView.OnSelectData.Subscribe(async data =>
            {
                var selectedLayer = _rendererView.SelectedLayer;
                Action<VideoSceneManager.FrameInfo> onUpdateTime = null;
                var isVideo = data.Type == FootageType.Video;
                if (isVideo) onUpdateTime = info => _rendererView.SetSeekSlider(selectedLayer, info.Rate);
                // 読み込み
                var subSceneController = await _layerManager.LoadFootage(data, selectedLayer, onUpdateTime);
                _rendererView.UpdateLayerView(selectedLayer, showSeekBar: isVideo, speed: 1f, attack: 0f);
                _rendererView.SetSeekSlider(selectedLayer, 0);
                // シーンのコントローラを配置
                if (subSceneController != null)
                {
                    subSceneController.transform.SetParent(_subSceneControllerParent, worldPositionStays: false);
                    // subSceneController が InputField を持っている場合、入力中はショートカットを効かないようにする
                    if (subSceneController is IHasInputField inputSubScene)
                    {
                        inputSubScene.OnStartInput.Subscribe(_ => _keyBinder.IsActive = false);
                        inputSubScene.OnFinishInput.Subscribe(_ => _keyBinder.IsActive = true);
                    }
                }
                // selectedLayer が変わったら表示も変える
                _rendererView.ObservableSelectedLayer.Subscribe(layer => _layerManager.ShowSubSceneController(layer).Forget());
            });
        }

        public void SendAttack(float value)
        {
            _rendererView.UpdateLayerView(_rendererView.SelectedLayer, attack: value);
            _layerManager.SendAttack(_rendererView.SelectedLayer, value);
        }

        public void SendSpeed(Layers layer, float value)
        {
            var speed = Mathf.Lerp(0, 5, value);
            _rendererView.UpdateLayerView(layer, speed: speed);
            _layerManager.SendSpeed(layer, speed);
        }

        public void SendVariable(SubSceneVariable variable, float value) => _layerManager.SendVariable(_rendererView.SelectedLayer, variable, value);

        public void SetBlendingFactor(Layers layer, float value) => _rendererView.SetBlendingSlider(layer, value);

        public void UnloadSelectedScene() => _layerManager.UnloadSceneAsync(_rendererView.SelectedLayer).Forget();

        public void FlashSelectedScene()
        {
            _flashingLayer = _rendererView.SelectedLayer;
            _flashTween.OnUpdate = value => _rendererView.SetBlendingSlider(_flashingLayer, value);
            _flashTween.Play();
        }
    }
}