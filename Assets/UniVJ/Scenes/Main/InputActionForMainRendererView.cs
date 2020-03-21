using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionForMainRendererView : MonoBehaviour
{
    [SerializeField] MainRendererView _mainRendererView;

    #region ForSingleEvent
    public void SetSelectedLayer1() => _mainRendererView.SetSelectedLayer(Layers.Layer1);
    public void SetSelectedLayer2() => _mainRendererView.SetSelectedLayer(Layers.Layer2);
    public void SetSelectedLayer3() => _mainRendererView.SetSelectedLayer(Layers.Layer3);
    public void SetSelectedLayer4() => _mainRendererView.SetSelectedLayer(Layers.Layer4);
    #endregion
}