using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
using UnityEngine.EventSystems;

public class TextField : MonoBehaviour, ISelectHandler
{
    [SerializeField] private TMP_InputField _inputField;
    public IObservable<Unit> OnFocus => _onFocus;
    public IObservable<Unit> OnEndInput => _onEndInput;
    public IObservable<string> OnEdit => _onEdit;
    private Subject<string> _onEdit = new Subject<string>();
    private Subject<Unit> _onFocus = new Subject<Unit>();
    private Subject<Unit> _onEndInput = new Subject<Unit>();
    public bool IsFocused => _inputField.isFocused;

    void Awake()
    {
        _inputField.onValueChanged.AddListener(_onEdit.OnNext);
        _inputField.onEndEdit.AddListener(_ => _onEndInput.OnNext(Unit.Default));
    }

    public void OnSelect(BaseEventData data) => _onFocus.OnNext(Unit.Default);

    void OnDestroy() => removeFocusIfNeed();

    private void OnDisable() => removeFocusIfNeed();

    private void removeFocusIfNeed()
    {
        if (IsFocused) _onEndInput.OnNext(Unit.Default);
    }

    public void SetText(string value) => _inputField.text = value;
}