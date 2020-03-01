/// <summary>
/// 素材リストのデータ。素材の情報を保持する。
/// </summary>
public class FootageScrollViewData
{
    public string SceneName { get; }
    public int Index { get; }

    public FootageScrollViewData(string sceneName)
    {
        SceneName = sceneName;
    }
}

