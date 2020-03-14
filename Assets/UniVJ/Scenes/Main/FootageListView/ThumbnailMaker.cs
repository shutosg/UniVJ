using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Async;
using Zenject;

/// <summary>
/// サムネイルを作成する。 EnqueueMakeThumbnail() を呼ぶと勝手に作成を開始する。
/// サムネイル撮影用の VideoScene を読み込んで撮影し、png保存と完了時処理を実行する。
/// </summary>
public class ThumbnailMaker
{
    private class TaskData
    {
        public string FilePath { get; set; }
        public Action<Texture2D> OnMake { get; set; }
        public CancellationToken Token { get; set; }
        public TaskData(string filePath, Action<Texture2D> onMake, CancellationToken token)
        {
            FilePath = filePath;
            OnMake = onMake;
            Token = token;
        }
    }
    private VideoSceneManager _videoSceneManager;
    private RenderTexture _renderTexture = new RenderTexture(768, 432, 0);
    private Queue<TaskData> _makeTasks = new Queue<TaskData>();
    private bool _hasStartedTask;

    [Inject] private LayerManager _layerManager;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    private async UniTask initialize()
    {
        _videoSceneManager = await _layerManager.LoadThumbnailMakerScene();
        _videoSceneManager.Setup(_renderTexture, Layers.ThumbnailMaker);
    }

    /// <summary>
    /// サムネイルが存在するか確認する
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public bool HasThumbnail(string filePath) => File.Exists(GetThumbnailPath(filePath));

    /// <summary>
    /// サムネイルのファイルパスを取得する
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public string GetThumbnailPath(string filePath)
    {
        filePath = filePath.Replace(Path.GetExtension(filePath), ".png");
        return filePath.Replace(FootageManager.FootagePath, FootageManager.ThumbnailPath);
    }

    /// <summary>
    /// サムネイル作成タスクを追加する
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="onMake"></param>
    /// <param name="token"></param>
    public void EnqueueMakeThumbnail(string filePath, Action<Texture2D> onMake, CancellationToken token)
    {
        _makeTasks.Enqueue(new TaskData(filePath, onMake, token));
        if (!_hasStartedTask) startMakeTask().Forget();
    }

    /// <summary>
    /// サムネイル作成タスクを開始する
    /// </summary>
    /// <returns></returns>
    private async UniTask startMakeTask()
    {
        if (_hasStartedTask) return;
        _hasStartedTask = true;
        while(_makeTasks.Count > 0)
        {
            var data = _makeTasks.Dequeue();
            try {
                await makeThumbnail(data.FilePath, data.OnMake, data.Token);
            } catch (OperationCanceledException) { }
        }
        await _layerManager.UnloadSceneAsync(Layers.ThumbnailMaker);
        _videoSceneManager = null;
        _hasStartedTask = false;
    }

    /// <summary>
    /// サムネイルを作成する
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="onMake"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private async UniTask makeThumbnail(string filePath, Action<Texture2D> onMake, CancellationToken token)
    {
        // 初期化されてなければ初期化する
        if (_videoSceneManager == null)
        {
            await initialize();
        }
        token.ThrowIfCancellationRequested();
        // 動画読み込み
        await _videoSceneManager.LoadVideo(filePath);
        await _videoSceneManager.SetSeekValue(UnityEngine.Random.Range(0f, 1f));
        await _videoSceneManager.Pause();
        // 描画完了まで待つ
        await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
        // サムネイル用テクスチャ作成
        var tex = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RGB24, false);
        var cachedRenderTexture = RenderTexture.active;
        RenderTexture.active = _renderTexture;
        tex.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
        tex.Apply();
        RenderTexture.active = cachedRenderTexture;
        // 保存
        (new FileInfo(FootageManager.ThumbnailPath + filePath.Remove(0, FootageManager.FootagePath.Length))).Directory.Create();
        var savePath = GetThumbnailPath(filePath);
        System.IO.File.WriteAllBytes(savePath, tex.EncodeToPNG());
        token.ThrowIfCancellationRequested();
        onMake(tex);
    }
}
