using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class KeyInputBinder : MonoBehaviour, IKeyInputBinder
{
    [SerializeField] private EventDictionary _events;
    private KeyValuePair<KeyCode, UnityEvent>[] _keyValuePairs;
    public bool IsActive { get; set; } = true;

    void Start()
    {
        _keyValuePairs = _events.ToArray();
    }

    void Update()
    {
        if (!IsActive) return;
        foreach (var keyValuePair in _keyValuePairs)
        {
            if (Input.GetKey(keyValuePair.Key))
            {
                keyValuePair.Value.Invoke();
            }
        }
    }
}

[Serializable] public class EventDictionary : SerializableDictionary<KeyCode, UnityEvent> { }

public interface IKeyInputBinder
{
    bool IsActive { get; set; }
}