using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Renderer : MonoBehaviour
{
    [SerializeField] private RawImage _mainImage;
    private List<RenderTexture> _subSceneRenderTextures = new List<RenderTexture>();
    [SerializeField] private Shader _mixShader;
    private Material _mixMaterial;

    public void Initialize()
    {
        _mixMaterial = new Material(_mixShader);
        _mainImage.material = _mixMaterial;
        for (var i = 0; i < 4; i++)
        {
            var rt = new RenderTexture(1920, 1080, 0);
            _subSceneRenderTextures.Add(rt);
            _mixMaterial.SetTexture($"_Tex{i + 1}", rt);
        }
    }

    public void RegistorySubScene(SubSceneCamera camera, int index)
    {
        camera.Setup(_subSceneRenderTextures[index], (Layers)(11 + index));
    }

    public void SetFadeValue(int index, float fadeValue)
    {
        _mixMaterial.SetFloat($"_BlendingFactor{index + 1}", fadeValue);
    }
}
