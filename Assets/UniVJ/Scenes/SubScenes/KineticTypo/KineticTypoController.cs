using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using KineticTypo;
using ModestTree;

public class KineticTypoController : SubSceneController, IHasInputField
{
    [SerializeField] private TextField _inputField;
    [SerializeField] private Toggle _autoLoop;
    [SerializeField] private TMP_Dropdown _fontSelector;
    [SerializeField] private TMP_Dropdown _animSelector;
    private KineticTypoManager _kineticTypoManager;
    public IObservable<Unit> OnStartInput => _inputField.OnFocus;
    public IObservable<Unit> OnFinishInput => _inputField.OnEndInput;
    public bool Inputting => _inputField.IsFocused;

    protected override bool initialize()
    {
        if (!tryCastSubSceneManager(out _kineticTypoManager)) return false;
        _inputField.OnEdit.Subscribe(value =>
        {
            if (value.IndexOf('\n') == -1 || value.Length == 1) return;
            _kineticTypoManager.PlayAnimation(value.Replace("\n", ""));
            _inputField.SetText("");
        });
        // アニメーション速度初期化
        _kineticTypoManager.OnReceiveSpeed(1f);
        _autoLoop.OnValueChangedAsObservable().Subscribe(_kineticTypoManager.SetLoop);
        var (fonts, materials, animations) = _kineticTypoManager.Initialize();
        _fontSelector.options = fonts.Select(f => new TMP_Dropdown.OptionData { text = f.name }).ToList();
        _fontSelector.onValueChanged.AddListener(index => _kineticTypoManager.SetFont(fonts[index]));
        _animSelector.options = animations
            .Select(a =>
            {
                var animationName = a.AnimationName.IsEmpty() ? nameof(a).Replace("Animation", "") : a.AnimationName;
                return new TMP_Dropdown.OptionData { text = animationName };
            })
            .ToList();
        _animSelector.onValueChanged.AddListener(_kineticTypoManager.SetAnimation);
        return true;
    }
}