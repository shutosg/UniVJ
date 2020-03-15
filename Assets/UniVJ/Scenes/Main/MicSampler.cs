using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicSampler : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField, Range(0f, 10f)] float _gain = 1f; // 音量に掛ける倍率

    private float[] _volumeRates = new float[1024];
    void Start()
    {
        Debug.Log(Microphone.devices.Length);
        if ((Microphone.devices.Length > 0))
        {
            source.clip = Microphone.Start(null, true, 10, 44100);
            while (!(Microphone.GetPosition(null) > 0)) {}
            source.Play();
        }
    }

    void Update()
    {
        source.GetOutputData(_volumeRates, 1);
        for(var i = 0; i < _volumeRates.Length; i++)
        {
            _volumeRates[i] *= _gain;
        }
        _volumeRates.DebugLog();
    }
}