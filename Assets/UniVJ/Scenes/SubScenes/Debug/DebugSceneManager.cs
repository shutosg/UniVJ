using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DebugSceneManager : SubSceneManager
{
    [SerializeField] private TextMeshProUGUI _displayNum;
    [SerializeField] private TextMeshProUGUI _micNum;
    [SerializeField] private TextMeshProUGUI _inputDeviceNm;
    [SerializeField] private TextMeshProUGUI _latestInputValue;

    private void Update()
    {
        _displayNum.SetText($"DisplayNum: {Display.displays.Length}");
        _micNum.SetText($"MicNum: {Microphone.devices.Length}");
        _inputDeviceNm.SetText($"InputDeviceNum: {InputSystem.devices.Count}");
    }

    public override void OnReceiveAttack(float value)
    {
        _latestInputValue.SetText($"LatestInputValue[time: {Time.time}]: {value}");
    }

    public override void OnReceiveSpeed(float value)
    {
        _latestInputValue.SetText($"LatestInputValue[time: {Time.time}]: {value}");
    }
}