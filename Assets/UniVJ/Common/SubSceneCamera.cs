using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSceneCamera : MonoBehaviour
{
    [SerializeField] private Camera _sceneCamera;
    [SerializeField] private Light[] _sceneLights;
    private RenderTexture _targetTexture;
    [SerializeField] private Shader _postEffectShader;
    private Material _renderMaterial;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="targetTexture">描画先のテクスチャ</param>
    /// <param name="layer">このシーンのレイヤー</param>
    public void Setup(RenderTexture targetTexture, Layers layer)
    {
        _targetTexture = targetTexture;
        _renderMaterial = new Material(_postEffectShader);
        _sceneCamera.targetTexture = targetTexture;

        // レイヤーを設定
        _sceneCamera.cullingMask = layer.ToInt();
        foreach(var l in _sceneLights)
        {
            l.cullingMask = layer.ToInt();
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if(_renderMaterial == null) return;
        Graphics.Blit(src, _targetTexture, _renderMaterial);
    }
}
