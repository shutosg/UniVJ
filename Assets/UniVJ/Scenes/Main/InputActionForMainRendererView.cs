using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UniVJ.Input
{
    public class InputActionForMainRendererView : MonoBehaviour
    {
        [SerializeField] MainRendererView _mainRendererView;

        public void SetSelectedLayer(Layers layer) => _mainRendererView.SetSelectedLayer(layer);

        #region ForSingleEvent
        public void SetSelectedLayer1() => _mainRendererView.SetSelectedLayer(Layers.Layer1);
        public void SetSelectedLayer2() => _mainRendererView.SetSelectedLayer(Layers.Layer2);
        public void SetSelectedLayer3() => _mainRendererView.SetSelectedLayer(Layers.Layer3);
        public void SetSelectedLayer4() => _mainRendererView.SetSelectedLayer(Layers.Layer4);
        public void SetSelectedLayer5() => _mainRendererView.SetSelectedLayer(Layers.Layer5);
        public void SetSelectedLayer6() => _mainRendererView.SetSelectedLayer(Layers.Layer6);
        public void SetSelectedLayer7() => _mainRendererView.SetSelectedLayer(Layers.Layer7);
        #endregion
    }
}