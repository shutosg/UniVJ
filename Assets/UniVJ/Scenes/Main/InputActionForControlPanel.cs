using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionForControlPanel : MonoBehaviour
{
    [SerializeField] ControlPanel _controlPanel;

    public void SendSpeed1(InputAction.CallbackContext context) => _controlPanel.SendSpeed(Layers.Layer1, context);
    public void SendSpeed2(InputAction.CallbackContext context) => _controlPanel.SendSpeed(Layers.Layer2, context);
    public void SendSpeed3(InputAction.CallbackContext context) => _controlPanel.SendSpeed(Layers.Layer3, context);
    public void SendSpeed4(InputAction.CallbackContext context) => _controlPanel.SendSpeed(Layers.Layer4, context);

    public void SetBlendingFactor1(InputAction.CallbackContext context)
        => _controlPanel.SetBlendingFactor(Layers.Layer1, context.ReadValue<float>());

    public void SetBlendingFactor2(InputAction.CallbackContext context)
        => _controlPanel.SetBlendingFactor(Layers.Layer2, context.ReadValue<float>());

    public void SetBlendingFactor3(InputAction.CallbackContext context)
        => _controlPanel.SetBlendingFactor(Layers.Layer3, context.ReadValue<float>());

    public void SetBlendingFactor4(InputAction.CallbackContext context)
        => _controlPanel.SetBlendingFactor(Layers.Layer4, context.ReadValue<float>());

    public void SendAttack(InputAction.CallbackContext context) => _controlPanel.SendAttack(context.ReadValue<float>());
}