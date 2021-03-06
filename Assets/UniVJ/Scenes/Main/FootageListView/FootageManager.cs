﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UniRx.Async;
using UnityEditor;

namespace UniVJ
{
    /// <summary>
    /// 素材マネージャ。素材の読み込みやリスティングを行なう。
    /// </summary>
    public class FootageManager
    {
        public static readonly string UniVJDocumentsPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/UniVJ";
        public static readonly string FootagePath = $"{UniVJDocumentsPath}/Footages/";
        public static readonly string ThumbnailPath = $"{UniVJDocumentsPath}/Footages/.thumbnails/";
        public static readonly string FontsPath = "Assets/UniVJ/Fonts";
        private static readonly string[] TargetExtensions = new string[] { ".mp4", ".mov", ".png", ".jpg", ".jpeg", ".gif" };
        private static readonly Dictionary<string, Texture2D> imageCache = new Dictionary<string, Texture2D>();

        /// <summary>
        /// 素材リストに表示するためのすべての素材データを返す
        /// </summary>
        public IEnumerable<FootageScrollViewData> GetAllFootageData()
            => GetAllScenePaths().Select(path => (path: path, name: Path.GetFileNameWithoutExtension(path)))
                .Where(x => x.name != "Main")
                .Where(x => x.name != "Video")
                .Where(x => x.name != "Image")
                .Select(x => new FootageScrollViewData(x.name, x.path))
                .Concat(GetFootagePathData());

        /// <summary>
        /// 素材ディレクトリに存在する対象のメディアファイルの素材データを返す
        /// </summary>
        public IEnumerable<FootageScrollViewData> GetFootagePathData()
            => Directory.EnumerateFiles(FootagePath, "*", SearchOption.AllDirectories)
                .Where(path => path.IndexOf(ThumbnailPath) == -1)
                .Select(path => (relativePath: path, extension: Path.GetExtension(path)))
                .Where(x => TargetExtensions.Contains(x.extension))
                .Select(x
                    =>
                {
                    var type = FootageType.Image;
                    if (x.extension == ".mp4" || x.extension == ".mov") type = FootageType.Video;
                    return new FootageScrollViewData(x.relativePath, x.relativePath, x.relativePath.Substring(FootagePath.Length), type);
                });

        /// <summary>
        /// TMPro のフォントアセットを読み込む
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TMP_FontAsset> GetFonts()
            => AssetDatabase.FindAssets("t:TMP_FontAsset", new[] { FontsPath })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<TMP_FontAsset>);

        /// <summary>
        ///　Font Material を読み込む 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Material> GetFontMaterials()
            => AssetDatabase.FindAssets("t:Material", new[] { FontsPath })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<Material>);

        /// <summary>
        /// ビルドに含まれるすべてのシーンパスを返す
        /// </summary>
        public IEnumerable<string> GetAllScenePaths()
            => Enumerable.Range(0, SceneManager.sceneCountInBuildSettings)
                .Select(SceneUtility.GetScenePathByBuildIndex);

        /// <summary>
        /// 素材フォルダにある画像をテクスチャとして読み込む
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="token"></param>
        public async UniTask<Texture2D> LoadTexture(string fileName, CancellationToken token = default(CancellationToken))
        {
            if (imageCache.ContainsKey(fileName))
            {
                // 読込完了 or 読み込み失敗 まで待つ
                await UniTask.WaitUntil(() => !imageCache.ContainsKey(fileName) || imageCache[fileName] != null, cancellationToken: token);
                token.ThrowIfCancellationRequested();
                if (imageCache.ContainsKey(fileName))
                    return imageCache[fileName];
            }

            // キャッシュを空で追加(読込中の意味)
            imageCache.Add(fileName, null);

            // 読み込み開始
            using (var uwr = UnityWebRequestTexture.GetTexture("file://" + FootagePath + fileName))
            {
                await uwr.SendWebRequest();
                // 失敗したらキャッシュ消して抜ける
                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    imageCache.Remove(fileName);
                    Debug.LogError($"{uwr.url} が存在しない");
                    return null;
                }
                var tex = DownloadHandlerTexture.GetContent(uwr);
                imageCache[fileName] = tex;
                try
                {
                    token.ThrowIfCancellationRequested();
                }
                catch (OperationCanceledException) { }
                return tex;
            }
        }

        public async UniTask<byte[]> LoadBinary(string fileName, CancellationToken token = default(CancellationToken))
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
}