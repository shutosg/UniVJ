public enum Layers
{
    Default = 0,
    UI = 5,
    Scene1 = 11,
    Scene2 = 12,
    Scene3 = 13,
    Scene4 = 14,
    Scene5 = 15,
}

public static class LayersExtension
{
    public static int ToInt(this Layers self) => 1 << (int)self;
}