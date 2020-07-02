using System;
using UnityEngine;

namespace KineticTypo
{
    /// <summary>
    /// 1文字ごとのアニメーション用パラメータ
    /// </summary>
    public class AnimationData
    {
        private const int VertexCountPerChar = 4;
        /// <summary>初期座標オフセット</summary>
        public Vector3 StartPositionOffset { get; private set; }
        /// <summary>開始時間オフセット</summary>
        public float TimeOffset { get; private set; }
        /// <summary>元の座標(4頂点分)</summary>
        public Vector3[] SrcVertices { get; private set; }
        /// <summary>アニメーション中の座標(4頂点分)</summary>
        public Vector3[] CurrentVertices { get; private set; }
        /// <summary>元の頂点カラー(4頂点分)</summary>
        public Color32[] SrcColors { get; private set; }
        /// <summary>アニメーション中の頂点カラー(4頂点分)</summary>
        public Color32[] CurrentColors { get; private set; }
        /// <summary>アニメーションの現在時刻</summary>
        public float CurrentTime { get; set; }
        /// <summary>この文字のアニメーションが完了してるか</summary>
        public bool IsCompleted => CurrentTime > _duration;
        private float _duration;

        public AnimationData(Vector3 startPositionOffset, float timeOffset, float duration, Vector3[] vertices, int startIndex, Color32[] color32s)
            => Reset(startPositionOffset, timeOffset, duration, vertices, startIndex, color32s);

        public void Reset(Vector3 startPositionOffset, float timeOffset, float duration, Vector3[] vertices, int startIndex, Color32[] color32s)
        {
            _duration = duration;
            StartPositionOffset = startPositionOffset;
            TimeOffset = timeOffset;
            CurrentTime = 0f;
            if (SrcVertices == null)
            {
                SrcVertices = new Vector3[VertexCountPerChar];
            }
            Array.Copy(vertices, startIndex, SrcVertices, 0, VertexCountPerChar);
            if (CurrentVertices == null)
            {
                CurrentVertices = new Vector3[VertexCountPerChar];
            }
            Array.Copy(vertices, startIndex, CurrentVertices, 0, VertexCountPerChar);
            if (SrcColors == null)
            {
                SrcColors = new Color32[VertexCountPerChar];
            }
            Array.Copy(color32s, startIndex, SrcColors, 0, VertexCountPerChar);
            if (CurrentColors == null)
            {
                CurrentColors = new Color32[VertexCountPerChar];
            }
            Array.Copy(color32s, startIndex, CurrentColors, 0, VertexCountPerChar);
        }
    }
}