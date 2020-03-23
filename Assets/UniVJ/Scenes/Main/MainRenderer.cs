using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// 最終的な出力を行なう。出力に必要な RenderTexture(レイヤーの数だけ存在) を保持する。外部からの入力を受け付けて RenderTexture の合成を操作する。
/// </summary>
public class MainRenderer
{
    private List<RenderTexture> _subSceneRenderTextures = new List<RenderTexture>();
    private List<int> _shaderPropertyIDs = new List<int>();
    private Material _mixMaterial;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="mixShader">最終的な出力を行なうために使うシェーダ</param>
    public MainRenderer(MainRendererView view, Shader mixShader, Vector2Int resolution)
    {
        _mixMaterial = new Material(mixShader);
        for (var i = 0; i < 8; i++)
        {
            var rt = new RenderTexture(resolution.x, resolution.y, 0);
            _subSceneRenderTextures.Add(rt);
            _mixMaterial.SetTexture($"_Tex{i + 1}", rt);
            _shaderPropertyIDs.Add(Shader.PropertyToID($"_BlendingFactor{i + 1}"));
        }
        view.Initialize(_mixMaterial, _subSceneRenderTextures);
        // 入力を監視してパラメタを操作する
        view.OnChangeBlendingValues.ForEach((onChangeValue, i) => onChangeValue.Subscribe(v => SetFadeValue(i, v)));
        // デフォルトでレイヤー1を表示
        view.SetBlendingSlider(Layers.Layer1, 1);
    }

    /// <summary>
    /// 自身の持っている RenderTexture を合成元のシーンカメラの出力先に設定する
    /// </summary>
    /// <param name="manager">サブシーンマネージャ</param>
    /// <param name="layer">割り当てるレイヤー</param>
    public void RegistorySubScene(SubSceneManager manager, Layers layer)
    {
        if (layer < Layers.Layer1 || layer > Layers.Layer7) return;
        manager.Setup(_subSceneRenderTextures[(int)layer - (int)Layers.Layer1], layer);
    }

    /// <summary>
    /// ブレンディングに使う値を変更する
    /// </summary>
    /// <param name="index"></param>
    /// <param name="fadeValue"></param>
    public void SetFadeValue(int index, float fadeValue)
    {
        _mixMaterial.SetFloat(_shaderPropertyIDs[index], fadeValue);
    }
}