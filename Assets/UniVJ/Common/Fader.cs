using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Fader : Slider
{
    [SerializeField] private TextMeshProUGUI _valueText;

    public void SetValue(float value)
    {
        this.value = value;
        _valueText.SetText("{0:4}", value);
    }
}