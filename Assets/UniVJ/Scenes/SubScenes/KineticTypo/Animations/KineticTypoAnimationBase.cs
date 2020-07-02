using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UniRx.Async;
using TMPro;
using UnityEngine.Serialization;
using UniVJ.Utility;
using Zenject;


namespace KineticTypo
{
    public abstract class KineticTypoAnimationBase : MonoBehaviour
    {
        protected const float MinSpeed = 0.001f;
        protected const float DefaultDuration = 0.5f;
        [Inject(Id = InstallerId.MainTextId)] protected TextMeshPro _text;
        [SerializeField] protected AnimationCurve _positionCurve;
        [SerializeField] protected AnimationCurve _colorCurve;
        [SerializeField] public string AnimationName;
        public float Speed { get; protected set; } = 1f;
        public float IntervalText { get; protected set; } = 0.025f;
        private List<AnimationData> _animationDataList = new List<AnimationData>();
        private TMP_FontAsset _font;

        public void Initialize(TMP_FontAsset font) => _font = font;

        public async UniTask PlayAsync(CancellationToken token)
        {
            _text.ForceMeshUpdate();

            var textInfo = _text.textInfo;
            var charCount = textInfo.characterCount;
            var currentTime = Time.time;
            var startTime = Time.time;
            // カーブにキーが無ければ適当に打つ
            if (_positionCurve.keys.Length == 0)
            {
                _positionCurve.AddKey(new Keyframe(0, 0));
                _positionCurve.AddKey(new Keyframe(1, 1));
            }
            var curveDuration = _positionCurve.keys[_positionCurve.length - 1].time;

            // 初期設定
            for (var i = 0; i < charCount; i++)
            {
                var (positionOffset, timeOffset) = getStartState(i);
                var matIndex = textInfo.characterInfo[i].materialReferenceIndex;
                var charInfo = textInfo.characterInfo[i];
                var sourceVertices = textInfo.meshInfo[matIndex].vertices;
                var sourceColors = textInfo.meshInfo[matIndex].colors32;
                // 足りなかったら作る、足りるなら reset
                if (_animationDataList.Count <= i)
                {
                    var animData = new AnimationData(positionOffset, timeOffset, curveDuration, sourceVertices, charInfo.vertexIndex, sourceColors);
                    _animationDataList.Add(animData);
                }
                else
                {
                    _animationDataList[i].Reset(positionOffset, timeOffset, curveDuration, sourceVertices, charInfo.vertexIndex, sourceColors);
                }
            }
            // 多かったら消す
            if (_animationDataList.Count > charCount) _animationDataList.RemoveRange(charCount, _animationDataList.Count - charCount);


            // アニメーション
            while (true)
            {
                currentTime += Time.deltaTime;
                for (var i = 0; i < charCount; i++)
                {
                    token.ThrowIfCancellationRequested();
                    var animData = _animationDataList[i];
                    // 終了してたら抜ける
                    if (animData.IsCompleted) continue;
                    // 時刻更新
                    animData.CurrentTime = ((currentTime - startTime) - animData.TimeOffset) / (DefaultDuration / Speed);
                    var isStarted = animData.CurrentTime > 0f;
                    textInfo.characterInfo[i].isVisible = isStarted;
                    var charInfo = textInfo.characterInfo[i];
                    if (!charInfo.isVisible) continue;

                    // アニメーション開始してるなら計算
                    if (!isStarted) continue;
                    calculateNewState(animData);
                    var vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
                    var vertIdx = charInfo.vertexIndex;
                    var v = animData.CurrentVertices;
                    vertices[vertIdx + 0] = v[0];
                    vertices[vertIdx + 1] = v[1];
                    vertices[vertIdx + 2] = v[2];
                    vertices[vertIdx + 3] = v[3];

                    var colors = textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;
                    var c = animData.CurrentColors;
                    colors[vertIdx + 0] = c[0];
                    colors[vertIdx + 1] = c[1];
                    colors[vertIdx + 2] = c[2];
                    colors[vertIdx + 3] = c[3];
                }
                token.ThrowIfCancellationRequested();
                // 更新
                for (var i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                    _text.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
                }
                _text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

                if (_animationDataList.All(d => d.IsCompleted)) return;

                await UniTask.Yield();
            }
        }

        /// <summary>
        /// 各文字の初期座標の取得
        /// </summary>
        /// <param name="index">文字index</param>
        /// <returns>初期状態の座標オフセット、アニメーションの開始オフセット</returns>
        protected abstract (Vector3 positionOffset, float timeOffset) getStartState(int index);

        /// <summary>
        /// 各文字の座標を計算
        /// </summary>
        /// <param name="animData">対象の文字のAnimationData</param>
        protected abstract void calculateNewState(AnimationData animData);

        public void SetSpeed(float value) => Speed = Mathf.Max(value, MinSpeed);

        public void SetIntervalText(float value) => IntervalText = value;

        public void SetDuration(float value) => SetSpeed(DefaultDuration / value);
    }
}