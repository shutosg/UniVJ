using UnityEngine;
using Random = UnityEngine.Random;
using UniVJ.Utility;

namespace KineticTypo
{
    public class FromRandomAnimation : KineticTypoAnimationBase
    {
        protected override (Vector3 positionOffset, float timeOffset) getStartState(int index)
        {
            var timeOffset = index * IntervalText / Speed;
            var startX = 0f;
            while (Mathf.Abs(startX) < 5f) startX = Random.Range(-10f, 10f);
            var startY = 0f;
            while (Mathf.Abs(startY) < 2f) startY = Random.Range(-10f, 10f);
            var positionOffset = new Vector3(startX, startY, 0);
            return (positionOffset, timeOffset);
        }

        protected override void calculateNewState(AnimationData animData)
        {
            // 頂点座標
            var offset = MathUtility.Lerp(animData.StartPositionOffset, Vector3.zero, _positionCurve.Evaluate(animData.CurrentTime));
            var sv = animData.SrcVertices;
            var cv = animData.CurrentVertices;
            cv[0] = sv[0] + offset;
            cv[1] = sv[1] + offset;
            cv[2] = sv[2] + offset;
            cv[3] = sv[3] + offset;

            // 頂点カラー
            var cc = animData.CurrentColors;
            var alpha = (byte)Mathf.Lerp(0, 255, _colorCurve.Evaluate(animData.CurrentTime));
            cc[0].a = alpha;
            cc[1].a = alpha;
            cc[2].a = alpha;
            cc[3].a = alpha;
        }
    }
}