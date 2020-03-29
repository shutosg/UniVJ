namespace UniVJ
{
    /// <summary>
    /// 素材リストのデータ。素材の情報を保持する。
    /// </summary>
    public class FootageScrollViewData
    {
        public string FootageName { get; }
        public string DisplayName { get; }
        public string FootagePath { get; }
        public FootageType Type { get; }

        public FootageScrollViewData(string footageName, string footagePath, string displayName = "", FootageType type = FootageType.Scene)
        {
            FootageName = footageName;
            FootagePath = footagePath;
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
}