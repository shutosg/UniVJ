using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSceneControllerResolver : ISubSceneControllerResolver
{
    public SubSceneController GetSubSceneControllerPrefab(SubSceneManager manager)
    {
        // manager に対応している SubSceneController へのプレハブを返す
        var prefabName = "SubSceneControllers/" + manager.gameObject.scene.name;
        var prefab = Resources.Load<SubSceneController>(prefabName);
        if (prefab == null)
        {
            Debug.LogWarning($"prefab: {prefabName} が存在しません");
        }
        return prefab;
    }
}

/// <summary>
/// 与えられたシーンマネージャを制御可能なコントローラのプレハブを返す
/// </summary>
public interface ISubSceneControllerResolver
{
    SubSceneController GetSubSceneControllerPrefab(SubSceneManager manager);
}