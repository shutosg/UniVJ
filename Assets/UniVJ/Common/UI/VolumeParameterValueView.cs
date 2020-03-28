using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class VolumeParameterValueView<T> : MonoBehaviour
{
    public abstract void SetValue(T value);
    public virtual IObservable<T> OnValueChanged { get; }
}