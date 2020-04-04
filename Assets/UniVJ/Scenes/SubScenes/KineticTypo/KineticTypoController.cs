using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class KineticTypoController : SubSceneController, IHasInputField
{
    [SerializeField] private TextField _inputField;
    [SerializeField] private Toggle _autoLoop;
    [SerializeField] private TMP_Dropdown _fontSelector;
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
        var (fonts, materials) = _kineticTypoManager.Initialize();
        _fontSelector.options = fonts.Select(f => new TMP_Dropdown.OptionData {text = f.name}).ToList();
        _fontSelector.onValueChanged.AddListener(index => _kineticTypoManager.SetFont(fonts[index]));
        return true;
    }
}