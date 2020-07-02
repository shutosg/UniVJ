using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using UniRx.Async;
using UniVJ;
using Zenject;

namespace KineticTypo
{
    public class KineticTypoManager : SubSceneManager
    {
        [Inject] private FootageManager _footageManager;
        [Inject(Id = InstallerId.MainTextId)] private TextMeshPro _text;
        private CancellationTokenSource _source;
        private bool _isLoop;
        private float _intervalAnimation = 0.5f;
        private List<TMP_FontAsset> _fontAssets;
        private List<Material> _materials;
        private List<Type> _animationTypes;
        private KineticTypoAnimationBase _currentAnimation;
        private int _nextAnimationIndex;
        private KineticTypoAnimationBase[] _animations;

        public IObservable<Unit> OnAnimationCompleted => _onAnimationCompleted;
        private readonly Subject<Unit> _onAnimationCompleted = new Subject<Unit>();

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns></returns>
        public (List<TMP_FontAsset> Fonts, List<Material> Materials, KineticTypoAnimationBase[] Animations) Initialize()
        {
            _fontAssets = _footageManager.GetFonts().ToList();
            _materials = _footageManager.GetFontMaterials().ToList();
            _animations = GetComponents<KineticTypoAnimationBase>();
            return (_fontAssets, _materials, _animations);
        }

        public override void OnReceiveSpeed(float value) => _currentAnimation?.SetSpeed(value);
        protected override void onReceiveVariable1(float value) => _currentAnimation?.SetDuration(value);
        protected override void onReceiveVariable2(float value) => _currentAnimation?.SetIntervalText(value);
        protected override void onReceiveVariable3(float value) => _intervalAnimation = value;

        /// <summary>
        /// 再生
        /// </summary>
        /// <param name="text"></param>
        public void PlayAnimation(string text)
        {
            var nextAnimation = _animations[_nextAnimationIndex];
            // アニメーションパラメータを次のアニメーションに引き継ぐ
            if (_currentAnimation != null)
            {
                nextAnimation.SetSpeed(_currentAnimation.Speed);
                nextAnimation.SetIntervalText(_currentAnimation.IntervalText);
            }

            _currentAnimation = nextAnimation;

            _text.text = text;
            loopPlay().Forget();

            async UniTask loopPlay()
            {
                do
                {
                    await Replay();
                    await UniTask.Delay((int)(_intervalAnimation * 1000), cancellationToken: _source.Token);
                } while (_isLoop);
            }
        }

        public async UniTask Replay()
        {
            _source?.Cancel();
            _source = new CancellationTokenSource();
            await _currentAnimation.PlayAsync(_source.Token);
            _onAnimationCompleted.OnNext(Unit.Default);
        }

        public void SetFont(TMP_FontAsset font)
        {
            _text.font = font;
            // マテリアルが見つかれば割り当てる
            var targetMaterial = _materials.FirstOrDefault(m => m.name.IndexOf(font.name + "_OutlineOnly") != -1);
            if (targetMaterial != null) _text.fontSharedMaterial = targetMaterial;
        }

        public void SetAnimation(int index)
        {
            _nextAnimationIndex = index;
            PlayAnimation(_text.text);
        }

        public void SetLoop(bool isLoop) => _isLoop = isLoop;
    }
}