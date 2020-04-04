using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using UniRx.Async;
using UniVJ;
using UniVJ.Utility;
using Random = UnityEngine.Random;
using Zenject;

public class KineticTypoManager : SubSceneManager
{
    private const float MinSpeed = 0.001f;
    private const float DefaultDuration = 0.5f;
    private readonly Vector3 FarAway = Vector3.up * 100000f;
    [Inject] private FootageManager _footageManager;
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private AnimationCurve _curve;
    private CancellationTokenSource _source;
    private float _speed = 1f;
    private float _intervalText = 0.025f;
    private bool _isLoop;
    private float _intervalAnimation = 0.5f;
    private List<TMP_FontAsset> _fontAssets;
    private List<Material> _materials;

    public IObservable<Unit> OnAnimationCompleted => _onAnimationCompleted;
    private readonly Subject<Unit> _onAnimationCompleted = new Subject<Unit>();

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    public (List<TMP_FontAsset> Fonts, List<Material> Materials) Initialize()
    {
        _fontAssets = _footageManager.GetFonts().ToList();
        _materials = _footageManager.GetFontMaterials().ToList();
        return (_fontAssets, _materials);
    }

    public override void OnReceiveSpeed(float value) => setAnimationSpeed(value);
    protected override void onReceiveVariable1(float value) => setAnimationSpeed(DefaultDuration / value);
    protected override void onReceiveVariable2(float value) => _intervalText = value;
    protected override void onReceiveVariable3(float value) => _intervalAnimation = value;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    protected void setAnimationSpeed(float value) => _speed = Mathf.Max(value, MinSpeed);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    public void PlayAnimation(string text)
    {
        _text.text = text;
        loopPlay().Forget();

        async UniTask loopPlay() {
            do {
                await Replay();
                await UniTask.Delay((int)(_intervalAnimation * 1000));
            } while (_isLoop);
        }
    }

    public async UniTask Replay()
    {
        _source?.Cancel();
        _source = new CancellationTokenSource();
        await play(_source.Token);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    private async UniTask play(CancellationToken token)
    {
        const int VertexCountPerChar = 4;
        // ジオメトリ更新
        _text.ForceMeshUpdate();

        var textInfo = _text.textInfo;
        var charCount = textInfo.characterCount;
        var currentTime = Time.time;
        var startTime = Time.time;
        // 文字ごとのアニメーションが最後まで再生されたか
        var completes = new bool[charCount];
        var posOffsets = new Vector3[charCount];
        var startOffsets = new float[charCount];
        var sourceVertices = new Vector3[charCount][];
        var curveDuration = _curve.keys[_curve.length - 1].time;

        // 初期設定
        for (var i = 0; i < charCount; i++)
        {
            startOffsets[i] = i * _intervalText / _speed;
            var startX = 0f;
            while (Mathf.Abs(startX) < 5f) startX = Random.Range(-10f, 10f);
            var startY = 0f;
            while (Mathf.Abs(startY) < 2f) startY = Random.Range(-10f, 10f);
            posOffsets[i] = new Vector3(startX, startY, 0);
            var matIndex = textInfo.characterInfo[i].materialReferenceIndex;
            var charInfo = textInfo.characterInfo[i];
            sourceVertices[i] = new Vector3[VertexCountPerChar];
            Array.Copy(textInfo.meshInfo[matIndex].vertices, charInfo.vertexIndex, sourceVertices[i], 0, VertexCountPerChar);
        }

        // アニメーション
        while (true)
        {
            token.ThrowIfCancellationRequested();
            currentTime += Time.deltaTime;
            for (var i = 0; i < charCount; i++)
            {
                // 非表示なら無視
                var charInfo = textInfo.characterInfo[i];
                var animationTime = ((currentTime - startTime) - startOffsets[i]) / (DefaultDuration / _speed);
                var isStarted = animationTime > 0f;
                completes[i] = MathUtility.NearlyEquals(animationTime, curveDuration) || animationTime > curveDuration;
                if (!charInfo.isVisible) continue;

                var offset = isStarted ? MathUtility.Lerp(posOffsets[i], Vector3.zero, _curve.Evaluate(animationTime)) : FarAway;
                var vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
                var vertIdx = charInfo.vertexIndex;
                var v = sourceVertices[i][0];
                vertices[vertIdx + 0] = sourceVertices[i][0] + offset;
                vertices[vertIdx + 1] = sourceVertices[i][1] + offset;
                vertices[vertIdx + 2] = sourceVertices[i][2] + offset;
                vertices[vertIdx + 3] = sourceVertices[i][3] + offset;
            }
            // 更新
            for (var i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                _text.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            if (completes.All(completed => completed))
            {
                _onAnimationCompleted.OnNext(Unit.Default);
                return;
            }
            await UniTask.Yield();
        }
    }

    public void SetFont(TMP_FontAsset font)
    {
        _text.font = font;
        // マテリアルが見つかれば割り当てる
        var targetMaterial = _materials.FirstOrDefault(m => m.name.IndexOf(font.name + "_OutlineOnly") != -1);
        if (targetMaterial != null) _text.fontSharedMaterial = targetMaterial;
    }

    public void SetLoop(bool isLoop) => _isLoop = isLoop;
}

public enum KineticType
{
    Simple,
}