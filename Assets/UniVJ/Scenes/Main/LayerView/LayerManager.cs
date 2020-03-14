using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx.Async;

/// <summary>
/// レイヤーマネージャ。シーンの読み込みや破棄を管理する。
/// </summary>
public class LayerManager
{
    private MainRenderer _mainRenderer;
    private Dictionary<Layers, Scene> loadedScenes = new Dictionary<Layers, Scene>();
    private Dictionary<Layers, SubSceneManager> loadedSubSceneManagers = new Dictionary<Layers, SubSceneManager>();
    private Dictionary<Layers, bool> isLocking = new Dictionary<Layers, bool>();

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="mainRenderer"></param>
    public LayerManager(MainRenderer mainRenderer)
    {
        _mainRenderer = mainRenderer;
    }

    /// <summary>
    /// フッテージの読み込み
    /// </summary>
    /// <param name="data"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public async UniTask LoadFootage(FootageScrollViewData data, Layers layer, Action<float> onUpdateTime = null)
    {
        // シーンの読み込みと初期化
        switch(data.Type) {
            case FootageType.Scene:
                await loadSceneAsync(data.FootageName, layer);
                break;
            case FootageType.Video:
                await loadSceneAsync("Video", layer);
                var videoSceneManager = loadedSubSceneManagers[layer] as VideoSceneManager;
                videoSceneManager.LoadVideo(data.FootagePath, onUpdateTime);
                break;
            case FootageType.Image:
                await loadSceneAsync("Image", layer);
                var imageSceneManager = loadedSubSceneManagers[layer] as ImageSceneManager;
                imageSceneManager.LoadImage(data.DisplayName);
                break;
        }
    }

    public void SetSeekValue(Layers layer, float value) => loadedSubSceneManagers[layer].SetSeekValue(value);

    /// <summary>
    /// シーンを読み込む
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    /// <param name="layer">追加先のレイヤー</param>
    private async UniTask loadSceneAsync(string sceneName, Layers layer)
    {
        // 対象のレイヤーが処理中なら無視
        if (isLocking.ContainsKey(layer)) return;

        // 既にシーンが読み込まれていたら破棄する
        if (loadedScenes.ContainsKey(layer))
        {
            await UnloadSceneAsync(layer);
        }

        // FIXME: うまいやり方
        switch (layer)
        {
            case Layers.Layer1:
                SceneManager.sceneLoaded += onLoadedSceneAsLayer1;
                break;
            case Layers.Layer2:
                SceneManager.sceneLoaded += onLoadedSceneAsLayer2;
                break;
            case Layers.Layer3:
                SceneManager.sceneLoaded += onLoadedSceneAsLayer3;
                break;
            case Layers.Layer4:
                SceneManager.sceneLoaded += onLoadedSceneAsLayer4;
                break;
            default:
                break;
        }

        // 処理中フラグを立てて読み込み開始
        isLocking.Add(layer, true);
        await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        loadedSubSceneManagers.Add(layer, loadedScenes[layer].GetRootGameObjects()[0].GetComponent<SubSceneManager>());
    }

    public async UniTask UnloadSceneAsync(Layers layer)
    {
        // 処理中 or そもそも読み込まれていないなら無視
        if (isLocking.ContainsKey(layer) || !loadedScenes.ContainsKey(layer)) return;

        isLocking.Add(layer, true);
        await SceneManager.UnloadSceneAsync(loadedScenes[layer]);
        isLocking.Remove(layer);
        loadedScenes.Remove(layer);
        loadedSubSceneManagers.Remove(layer);
    }

    private void onLoadedSceneAsLayer1(Scene scene, LoadSceneMode mode)
    {
        isLocking.Remove(Layers.Layer1);
        loadedScenes.Add(Layers.Layer1, scene);
        setLayer(scene, Layers.Layer1);
        SceneManager.sceneLoaded -= onLoadedSceneAsLayer1;
    }

    private void onLoadedSceneAsLayer2(Scene scene, LoadSceneMode mode)
    {
        isLocking.Remove(Layers.Layer2);
        loadedScenes.Add(Layers.Layer2, scene);
        setLayer(scene, Layers.Layer2);
        SceneManager.sceneLoaded -= onLoadedSceneAsLayer2;
    }

    private void onLoadedSceneAsLayer3(Scene scene, LoadSceneMode mode)
    {
        isLocking.Remove(Layers.Layer3);
        loadedScenes.Add(Layers.Layer3, scene);
        setLayer(scene, Layers.Layer3);
        SceneManager.sceneLoaded -= onLoadedSceneAsLayer3;
    }

    private void onLoadedSceneAsLayer4(Scene scene, LoadSceneMode mode)
    {
        isLocking.Remove(Layers.Layer4);
        loadedScenes.Add(Layers.Layer4, scene);
        setLayer(scene, Layers.Layer4);
        SceneManager.sceneLoaded -= onLoadedSceneAsLayer4;
    }

    /// <summary>
    /// 読み込んだシーンの GameObject を対象のレイヤーに割り当てる。カメラやライトの CullingMask も対象のレイヤーにする。
    /// </summary>
    /// <param name="scene">読み込んだシーン</param>
    /// <param name="layer">対象のレイヤー</param>
    private void setLayer(Scene scene, Layers layer)
    {
        var rootObjects = scene.GetRootGameObjects();
        rootObjects.ForEach(go => go.SetLayerRecursively((int)layer));
        // NOTE: rootObjectの1つめにシーンマネージャが居る前提
        var subSceneManager = rootObjects[0].GetComponent<SubSceneManager>();
        _mainRenderer.RegistorySubScene(subSceneManager, layer);
    }
}
