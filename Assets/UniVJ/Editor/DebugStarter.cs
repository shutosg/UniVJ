using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class DebugStarter
{
    private static readonly string OpenedScenesPath = "openedScenes.txt";
    private static readonly string StartSceneName = "Main";

    /// <summary>
    /// コンストラクタ。エディタのイベントを登録する。
    /// </summary>
    static DebugStarter()
    {
        EditorApplication.playModeStateChanged += onStateChanged;
    }

    /// <summary>
    /// メインシーンを開く
    /// </summary>
    [MenuItem("DebugTools/DebugStart %#&p")]
    private static void DebugStart()
    {
        // 保存されてないシーンがあれば保存する
        var activeScene = EditorSceneManager.GetActiveScene();
        var openedScenes = Enumerable.Range(0, EditorSceneManager.sceneCount)
            .ToList()
            .Select(i => EditorSceneManager.GetSceneAt(i))
            .OrderByDescending(scene => scene == activeScene)
            .ToList();
        openedScenes.ForEach(scene => {
            if (scene.isDirty) EditorSceneManager.SaveScene(scene);
        });
        File.WriteAllText(OpenedScenesPath, string.Join(",", openedScenes.Select(scene => scene.path)));
        // メインシーン読み込み
        var mainScenePath = getAllSceneAndPathes()
           .FirstOrDefault(x => x.scene.name == StartSceneName).path;
        EditorSceneManager.OpenScene(mainScenePath, OpenSceneMode.Single);
        EditorApplication.isPlaying = true;

        // すべてのシーンを返す
        IEnumerable<(SceneAsset scene, string path)> getAllSceneAndPathes()
            => AssetDatabase.FindAssets("t:SceneAsset")
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Select(path => (obj: AssetDatabase.LoadAssetAtPath(path, typeof(SceneAsset)), path: path))
                .Where(x => x.obj != null)
                .Select(x => (scene: (x.obj as SceneAsset), path: x.path));
    }

    /// <summary>
    /// シーン変更時処理。エディタモードに戻ったことを検知して、もともと開いていたシーンを開き直す。
    /// </summary>
    /// <param name="state"></param>
    static void onStateChanged(PlayModeStateChange state)
    {
        if(state != PlayModeStateChange.EnteredEditMode) return;
        if(!File.Exists(OpenedScenesPath)) return;
        var rawText = File.ReadAllText(OpenedScenesPath);
        File.Delete(OpenedScenesPath);
        var openedScenePathes = rawText.Split(',');
        // もともと開いてたシーンを開き直す
        EditorSceneManager.OpenScene(openedScenePathes[0], OpenSceneMode.Single);
        openedScenePathes
            .Skip(1)
            .ToList()
            .ForEach(path => EditorSceneManager.OpenScene(path, OpenSceneMode.Additive));
    }
}
