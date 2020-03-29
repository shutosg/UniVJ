using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Async;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class SubSceneManager : MonoBehaviour
{
    [SerializeField] private Camera _sceneCamera;
    [SerializeField] private Light[] _sceneLights;
    [SerializeField] private Shader _postEffectShader;
    [SerializeField] private Volume _volume;
    private RenderTexture _targetTexture;
    private Material _renderMaterial;
    protected int _speedId;
    public ColorAdjustments ColorAdjustments { get; private set; }

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
        var hdCamera = _sceneCamera.GetComponent<HDAdditionalCameraData>();
        if (hdCamera != null)
        {
            hdCamera.volumeLayerMask = layer.ToFlagInt();
        }
        foreach (var l in _sceneLights)
        {
            l.cullingMask = layer.ToFlagInt();
        }

        // shader id 初期化
        _speedId = Shader.PropertyToID("Speed");
        if (_volume == null) return;
        if (_volume.profile.TryGet(out ColorAdjustments adj)) ColorAdjustments = adj;
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

    public float GetVariable(SubSceneVariable variable)
    {
        switch (variable)
        {
            case SubSceneVariable.Variable1: return getVariable1();
            case SubSceneVariable.Variable2: return getVariable2();
            case SubSceneVariable.Variable3: return getVariable3();
            case SubSceneVariable.Variable4: return getVariable4();
            default: return 0f;
        }
    }

    private void OnDestroy()
    {
        if (_volume == null) return;
        Destroy(_volume.profile);
    }

    protected virtual void onReceiveVariable1(float value) { }
    protected virtual float getVariable1() => 0;
    protected virtual void onReceiveVariable2(float value) { }
    protected virtual float getVariable2() => 0;
    protected virtual void onReceiveVariable3(float value) { }
    protected virtual float getVariable3() => 0;
    protected virtual void onReceiveVariable4(float value) { }
    protected virtual float getVariable4() => 0;
}