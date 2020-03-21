using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyInputBinder : MonoBehaviour
{
    [SerializeField] private EventDictionary _events;

    void Update()
    {
        foreach (var keyValuePair in _events)
        {
            if (Input.GetKey(keyValuePair.Key))
            {
                keyValuePair.Value.Invoke();
            }
        }
    }
}

[Serializable] public class EventDictionary : SerializableDictionary<KeyCode, UnityEvent> { }