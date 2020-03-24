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
    private readonly MainRenderer _mainRenderer;
    private readonly Dictionary<Layers, Scene> _loadedScenes = new Dictionary<Layers, Scene>();
    private readonly Dictionary<Layers, SubSceneManager> _loadedSubSceneManagers = new Dictionary<Layers, SubSceneManager>();
    private readonly Dictionary<Layers, bool> _isLocking = new Dictionary<Layers, bool>();

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
    /// <param name="onUpdateTime"></param>
    /// <returns></returns>
    public async UniTask LoadFootage(FootageScrollViewData data, Layers layer, Action<float> onUpdateTime = null)
    {
        // シーンの読み込みと初期化
        switch (data.Type)
        {
            case FootageType.Scene:
                await loadSceneAsync(data.FootageName, layer);
                break;
            case FootageType.Video:
                await loadSceneAsync("Video", layer);
                var videoSceneManager = _loadedSubSceneManagers[layer] as VideoSceneManager;
                videoSceneManager.LoadVideo(data.FootagePath, onUpdateTime).Forget();
                break;
            case FootageType.Image:
                await loadSceneAsync("Image", layer);
                var imageSceneManager = _loadedSubSceneManagers[layer] as ImageSceneManager;
                imageSceneManager.LoadImage(data.DisplayName);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SetSeekValue(Layers layer, float value) => _loadedSubSceneManagers[layer].SetSeekValue(value).Forget();

    public void SendAttack(Layers layer, float value)
    {
        if (!_loadedSubSceneManagers.ContainsKey((layer))) return;
        _loadedSubSceneManagers[layer].OnReceiveAttack(value);
    }

    public void SendSpeed(Layers layer, float value)
    {
        if (!_loadedSubSceneManagers.ContainsKey((layer))) return;
        _loadedSubSceneManagers[layer].OnReceiveSpeed(value);
    }

    public void SendVariable(Layers layer, SubSceneVariable variable, float value)
    {
        if (!_loadedSubSceneManagers.ContainsKey((layer))) return;
        _loadedSubSceneManagers[layer].OnReceiveVariable(variable, value);
    }

    public async UniTask<VideoSceneManager> LoadThumbnailMakerScene()
    {
        if (!_loadedSubSceneManagers.ContainsKey(Layers.ThumbnailMaker))
        {
            await loadSceneAsync("video", Layers.ThumbnailMaker);
        }
        return _loadedSubSceneManagers[Layers.ThumbnailMaker] as VideoSceneManager;
    }

    /// <summary>
    /// シーンを読み込む
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    /// <param name="layer">追加先のレイヤー</param>
    private async UniTask loadSceneAsync(string sceneName, Layers layer)
    {
        // 対象のレイヤーが処理中なら無視
        if (_isLocking.ContainsKey(layer)) return;

        // 既にシーンが読み込まれていたら破棄する
        if (_loadedScenes.ContainsKey(layer)) { await UnloadSceneAsync(layer); }

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
            case Layers.Layer5:
                SceneManager.sceneLoaded += onLoadedSceneAsLayer5;
                break;
            case Layers.Layer6:
                SceneManager.sceneLoaded += onLoadedSceneAsLayer6;
                break;
            case Layers.Layer7:
                SceneManager.sceneLoaded += onLoadedSceneAsLayer7;
                break;
            case Layers.ThumbnailMaker:
                SceneManager.sceneLoaded += onLoadedSceneAsThumbnailMaker;
                break;
            default:
                UnityEngine.Debug.LogError("未登録のレイヤーが指定された");
                break;
        }

        // 処理中フラグを立てて読み込み開始
        _isLocking.Add(layer, true);
        await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        _loadedSubSceneManagers.Add(layer,
            _loadedScenes[layer].GetRootGameObjects()[0].GetComponent<SubSceneManager>());
    }

    public async UniTask UnloadSceneAsync(Layers layer)
    {
        // 処理中 or そもそも読み込まれていないなら無視
        if (_isLocking.ContainsKey(layer) || !_loadedScenes.ContainsKey(layer)) return;

        _isLocking.Add(layer, true);
        await SceneManager.UnloadSceneAsync(_loadedScenes[layer]);
        _isLocking.Remove(layer);
        _loadedScenes.Remove(layer);
        _loadedSubSceneManagers.Remove(layer);
    }

    private void onLoadedSceneAsLayer1(Scene scene, LoadSceneMode mode)
    {
        _isLocking.Remove(Layers.Layer1);
        _loadedScenes.Add(Layers.Layer1, scene);
        setLayer(scene, Layers.Layer1);
        SceneManager.sceneLoaded -= onLoadedSceneAsLayer1;
    }

    private void onLoadedSceneAsLayer2(Scene scene, LoadSceneMode mode)
    {
        _isLocking.Remove(Layers.Layer2);
        _loadedScenes.Add(Layers.Layer2, scene);
        setLayer(scene, Layers.Layer2);
        SceneManager.sceneLoaded -= onLoadedSceneAsLayer2;
    }

    private void onLoadedSceneAsLayer3(Scene scene, LoadSceneMode mode)
    {
        _isLocking.Remove(Layers.Layer3);
        _loadedScenes.Add(Layers.Layer3, scene);
        setLayer(scene, Layers.Layer3);
        SceneManager.sceneLoaded -= onLoadedSceneAsLayer3;
    }

    private void onLoadedSceneAsLayer4(Scene scene, LoadSceneMode mode)
    {
        _isLocking.Remove(Layers.Layer4);
        _loadedScenes.Add(Layers.Layer4, scene);
        setLayer(scene, Layers.Layer4);
        SceneManager.sceneLoaded -= onLoadedSceneAsLayer4;
    }

    private void onLoadedSceneAsLayer5(Scene scene, LoadSceneMode mode)
    {
        _isLocking.Remove(Layers.Layer5);
        _loadedScenes.Add(Layers.Layer5, scene);
        setLayer(scene, Layers.Layer5);
        SceneManager.sceneLoaded -= onLoadedSceneAsLayer5;
    }

    private void onLoadedSceneAsLayer6(Scene scene, LoadSceneMode mode)
    {
        _isLocking.Remove(Layers.Layer6);
        _loadedScenes.Add(Layers.Layer6, scene);
        setLayer(scene, Layers.Layer6);
        SceneManager.sceneLoaded -= onLoadedSceneAsLayer6;
    }

    private void onLoadedSceneAsLayer7(Scene scene, LoadSceneMode mode)
    {
        _isLocking.Remove(Layers.Layer7);
        _loadedScenes.Add(Layers.Layer7, scene);
        setLayer(scene, Layers.Layer7);
        SceneManager.sceneLoaded -= onLoadedSceneAsLayer7;
    }

    private void onLoadedSceneAsThumbnailMaker(Scene scene, LoadSceneMode mode)
    {
        _isLocking.Remove(Layers.ThumbnailMaker);
        _loadedScenes.Add(Layers.ThumbnailMaker, scene);
        setLayer(scene, Layers.ThumbnailMaker);
        SceneManager.sceneLoaded -= onLoadedSceneAsThumbnailMaker;
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