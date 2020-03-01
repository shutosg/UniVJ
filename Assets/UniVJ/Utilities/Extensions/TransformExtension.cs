using UnityEngine;

public static class TransformExtension
{
    /// <summary>
    /// 座標に x を加える
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    public static void AddPositionX(this Transform self, float x) => self.position = self.position.AddX(x);
    public static void AddPositionY(this Transform self, float y) => self.position = self.position.AddY(y);
    public static void AddPositionZ(this Transform self, float z) => self.position = self.position.AddZ(z);

    /// <summary>
    /// 回転に x を加える
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    public static void AddRotationX(this Transform self, float x) => self.rotation = self.rotation.AddX(x);
    public static void AddRotationY(this Transform self, float y) => self.rotation = self.rotation.AddY(y);
    public static void AddRotationZ(this Transform self, float z) => self.rotation = self.rotation.AddZ(z);
    public static void AddRotationW(this Transform self, float w) => self.rotation = self.rotation.AddW(w);

    /// <summary>
    /// 回転に x を加える
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    public static void AddEulerAngleX(this Transform self, float x) => self.eulerAngles = self.eulerAngles.AddX(x);
    public static void AddEulerAngleY(this Transform self, float y) => self.eulerAngles = self.eulerAngles.AddY(y);
    public static void AddEulerAngleZ(this Transform self, float z) => self.eulerAngles = self.eulerAngles.AddZ(z);
}
