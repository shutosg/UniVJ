public enum Layers
{
    Default = 0,
    UI = 5,
    Layer1 = 11,
    Layer2 = 12,
    Layer3 = 13,
    Layer4 = 14,
    Layer5 = 15,
    Layer6 = 16,
    Layer7 = 17,
    ThumbnailMaker = 21,
}

public static class LayersExtension
{
    public static int ToFlagInt(this Layers self) => 1 << (int)self;
}