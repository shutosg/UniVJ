using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// サブシーンのコントローラクラス。シーンマネージャを操作するUIを提供する。
/// </summary>
public class SubSceneController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _footagePath;
    [SerializeField] private Fader[] _sliderVariables;
    private SubSceneManager _subSceneManager;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="subSceneManager"></param>
    /// <param name="footagePath"></param>
    /// <returns>初期化に成功したか</returns>
    public virtual bool Initialize(SubSceneManager subSceneManager, string footagePath)
    {
        _subSceneManager = subSceneManager;
        _footagePath.text = footagePath;
        _sliderVariables.ForEach((s, i) => s.onValueChanged.AddListener(v => subSceneManager.OnReceiveVariable(SubSceneVariable.Variable1 + i, v)));
        return initialize();
    }

    /// <summary>
    /// SubSceneManager を指定の型に Cast する。失敗したらエラー出す。
    /// </summary>
    /// <param name="result">cast結果</param>
    /// <typeparam name="T">castする型</typeparam>
    /// <returns>成功したか</returns>
    protected bool tryCastSubSceneManager<T>(out T result) where T : SubSceneManager
    {
        result = _subSceneManager as T;
        if (result != null) return true;
        // 失敗してたらエラー
        Debug.LogError($"{GetType().Name} は与えられた {_subSceneManager.GetType().Name} に対応していません");
        return false;
    }

    public void SetVariableSlider(int index, float value) => _sliderVariables[index].SetValue(value);

    protected virtual bool initialize() => true;

    public void SetActive(bool isActive) => gameObject.SetActive(isActive);
}