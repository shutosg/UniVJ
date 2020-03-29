using UnityEngine;
using UnityEngine.VFX;
using UniVJ.Utility;

public class ParticleTunnelManager : SubSceneManager
{
    [SerializeField] private VisualEffect[] _vfxs;
    private int _turbulencePowerId;
    private int _sizeId;
    private int _ampOffsetId;

    void Awake()
    {
        _turbulencePowerId = Shader.PropertyToID("TurbulencePower");
        _sizeId = Shader.PropertyToID("Size");
        _ampOffsetId = Shader.PropertyToID("AmpOffset");
    }

    protected override void onReceiveVariable1(float value)
    {
        foreach (var vfx in _vfxs)
        {
            vfx.SetFloat(_sizeId, value);
        }
    }

    protected override float getVariable1() => _vfxs[0].GetFloat(_sizeId);


    protected override void onReceiveVariable2(float value)
    {
        foreach (var vfx in _vfxs)
        {
            vfx.SetFloat(_ampOffsetId, value);
        }
    }

    protected override float getVariable2() => _vfxs[0].GetFloat(_ampOffsetId);

    protected override void onReceiveVariable3(float value)
    {
        foreach (var vfx in _vfxs)
        {
            vfx.SetFloat(_turbulencePowerId, Mathf.Lerp(-20, 20, value));
        }
    }

    protected override float getVariable3() => MathUtility.Map(_vfxs[0].GetFloat(_turbulencePowerId), -20, 20, 0, 1);

    public override void OnReceiveSpeed(float value)
    {
        foreach (var vfx in _vfxs)
        {
            vfx.SetFloat(_speedId, value);
        }
    }
}