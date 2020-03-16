using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public class SimpleTransform : MonoBehaviour
{
    [SerializeField] private float _rotationPerFrame;
    [SerializeField] private float _scaleAmplitude;

    private Transform _cacheTrans;
    private Transform _cachedTrans => _cacheTrans ? _cacheTrans : (_cacheTrans = transform);
    private Vector3 _scaleCache;

    private void Start()
    {
        _scaleCache = transform.localScale;
    }

    void Update()
    {
        _cachedTrans.AddEulerAngleY(_rotationPerFrame);
        _cachedTrans.localScale = _scaleCache.Add(Mathf.Sin(Time.time) * _scaleAmplitude);
    }
}