/// <summary>
/// 素材リストのデータ。素材の情報を保持する。
/// </summary>
public class FootageScrollViewData
{
    public string FootageName { get; }
    public string DisplayName { get; }
    public int Index { get; }
    public FootageType Type { get; }

    public FootageScrollViewData(string footageName, string displayName = "", FootageType type = FootageType.Scene)
    {
        FootageName = footageName;
        DisplayName = string.IsNullOrEmpty(displayName) ? footageName : displayName;
        Type = type;
    }
}

public enum FootageType
{
    Scene,
    Video,
    Image,
}