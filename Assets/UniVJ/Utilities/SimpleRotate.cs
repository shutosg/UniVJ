using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    [SerializeField] private float rotationPerFrame;

    private Transform _cache;
    private Transform cachedTrans => _cache ?? (_cache = transform);

    void Update()
    {
        cachedTrans.AddEulerAngleY(rotationPerFrame);
    }
}
