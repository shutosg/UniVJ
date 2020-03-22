using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Async;
using UnityEngine.Rendering.HighDefinition;

public class SubSceneManager : MonoBehaviour
{
    [SerializeField] private Camera _sceneCamera;
    [SerializeField] private Light[] _sceneLights;
    private RenderTexture _targetTexture;
    [SerializeField] private Shader _postEffectShader;
    private Material _renderMaterial;
    protected int _speedId;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="targetTexture">描画先のテクスチャ</param>
    /// <param name="layer">このシーンのレイヤー</param>
    public virtual void Setup(RenderTexture targetTexture, Layers layer)
    {
        _targetTexture = targetTexture;
        _renderMaterial = new Material(_postEffectShader);
        _sceneCamera.targetTexture = targetTexture;

        // レイヤーを設定
        _sceneCamera.cullingMask = layer.ToFlagInt();
        foreach (var l in _sceneLights)
        {
            l.cullingMask = layer.ToFlagInt();
        }

        // shader id 初期化
        _speedId = Shader.PropertyToID("Speed");
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (_renderMaterial == null) return;
        Graphics.Blit(src, _targetTexture, _renderMaterial);
    }

    public virtual async UniTask SetSeekValue(float value)
    {
        await UniTask.Yield();
    }

    public virtual void OnReceiveAttack(float value) { }
    public virtual void OnReceiveSpeed(float value) { }

    public void OnReceiveVariable(SubSceneVariable variable, float value)
    {
        switch (variable)
        {
            case SubSceneVariable.Variable1:
                onReceiveVariable1(value);
                break;
            case SubSceneVariable.Variable2:
                onReceiveVariable2(value);
                break;
            case SubSceneVariable.Variable3:
                onReceiveVariable3(value);
                break;
            case SubSceneVariable.Variable4:
                onReceiveVariable4(value);
                break;
            default:
                break;
        }
    }

    protected virtual void onReceiveVariable1(float value) { }
    protected virtual void onReceiveVariable2(float value) { }
    protected virtual void onReceiveVariable3(float value) { }
    protected virtual void onReceiveVariable4(float value) { }
}