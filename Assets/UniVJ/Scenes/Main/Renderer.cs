using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Renderer
{
    private List<RenderTexture> _subSceneRenderTextures = new List<RenderTexture>();
    private Material _mixMaterial;

    public void Initialize(Shader mixShader)
    {
        _mixMaterial = new Material(mixShader);
        for (var i = 0; i < 4; i++)
        {
            var rt = new RenderTexture(1920, 1080, 0);
            _subSceneRenderTextures.Add(rt);
            _mixMaterial.SetTexture($"_Tex{i + 1}", rt);
        }
    }

    public void InitializeView(RendererView view)
    {
        view.Initialize(_mixMaterial, _subSceneRenderTextures);
        view.OnChangeBlendingValues.ForEach((onChangeValue, i) => onChangeValue.Subscribe(v => SetFadeValue(i, v)));
        view.SetBlendingSlider(0, 1);
    }

    public void RegistorySubScene(SubSceneCamera camera, Layers layer)
    {
        camera.Setup(_subSceneRenderTextures[(int)layer - (int)Layers.Scene1], layer);
    }

    public void SetFadeValue(int index, float fadeValue)
    {
        _mixMaterial.SetFloat($"_BlendingFactor{index + 1}", fadeValue);
    }
}
