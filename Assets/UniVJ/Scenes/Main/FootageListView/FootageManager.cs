using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UniRx.Async;

/// <summary>
/// 素材マネージャ。素材の読み込みやリスティングを行なう。
/// </summary>
public class FootageManager
{
    private static readonly string FootagePath = "/Users/shuto/UniVJ/Footages/";
    private static readonly string[] TargetExtensions = new string[] { ".mp4", ".mov", ".png", ".jpg", ".jpeg", ".gif" };

    private static readonly Dictionary<string, Texture2D> imageCache = new Dictionary<string, Texture2D>();

    public static IEnumerable<FootageScrollViewData> GetAllFootageData()
        => GetAllSceneNames()
        .Where(name => name != "Main")
        .Where(name => name != "Video")
        .Where(name => name != "Image")
        .Select(name => new FootageScrollViewData(name))
        .Concat(GetFootagePathData())
        .ToList();

    public static IEnumerable<string> GetFootagePathes()
        => Directory.EnumerateFiles(FootagePath, "*", SearchOption.AllDirectories)
        .Select(path => (path: path, extension: Path.GetExtension(path)))
        .Where(x => TargetExtensions.Contains(x.extension))
        .Select(x => x.path);

    public static IEnumerable<FootageScrollViewData> GetFootagePathData()
        => Directory.EnumerateFiles(FootagePath, "*", SearchOption.AllDirectories)
        .Select(path => (path: path, extension: Path.GetExtension(path)))
        .Where(x => TargetExtensions.Contains(x.extension))
        .Select(x => {
            var type = FootageType.Image;
            if (x.extension == ".mp4" || x.extension == ".mov") type = FootageType.Video;
            return new FootageScrollViewData(x.path, x.path.Substring(FootagePath.Length), type);
        });

    public static IEnumerable<string> GetAllSceneNames() => Enumerable.Range(0, SceneManager.sceneCountInBuildSettings)
        .Select(i => SceneUtility.GetScenePathByBuildIndex(i))
        .Select(path => Path.GetFileNameWithoutExtension(path));

    public static async UniTask<Texture2D> LoadTexture(string fileName, CancellationToken token = default(CancellationToken))
    {
        if (imageCache.ContainsKey(fileName))
        {
            // 読込完了 or 読み込み失敗 まで待つ
            await UniTask.WaitUntil(() => !imageCache.ContainsKey(fileName) || imageCache[fileName] != null);
            token.ThrowIfCancellationRequested();
            if (imageCache.ContainsKey(fileName))
                return imageCache[fileName];
        }

        // キャッシュを空で追加(読込中の意味)
        imageCache.Add(fileName, null);

        // 読み込み開始
        using (var uwr = new UnityWebRequest("file://" + FootagePath + fileName))
        {
            var handler = new DownloadHandlerTexture();
            uwr.downloadHandler = handler;
            await uwr.SendWebRequest();
            token.ThrowIfCancellationRequested();
            // 失敗したらキャッシュ消して抜ける
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                imageCache.Remove(fileName);
                return null;
            }
            imageCache[fileName] = handler.texture;
            return handler.texture;
        }
    }

    public static async UniTask<byte[]> LoadBinary(string fileName, CancellationToken token = default(CancellationToken))
    {
        // 読み込み開始
        using (var uwr = new UnityWebRequest("file://" + FootagePath + fileName))
        {
            var handler = new DownloadHandlerBuffer();
            uwr.downloadHandler = handler;
            await uwr.SendWebRequest();
            token.ThrowIfCancellationRequested();
            // 失敗したらキャッシュ消して抜ける
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                imageCache.Remove(fileName);
                return null;
            }
            return handler.data;
        }
    }
}
