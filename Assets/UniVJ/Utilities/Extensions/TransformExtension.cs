using UnityEngine;

public static class TransformExtension
{
    /// <summary>
    /// 座標に x を加える
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    public static void AddPositionX(this Transform self, float v) => self.position = self.position.AddX(v);
    public static void AddPositionY(this Transform self, float v) => self.position = self.position.AddY(v);
    public static void AddPositionZ(this Transform self, float v) => self.position = self.position.AddZ(v);

    /// <summary>
    /// 回転に x を加える
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    public static void AddRotationX(this Transform self, float v) => self.rotation = self.rotation.AddX(v);
    public static void AddRotationY(this Transform self, float v) => self.rotation = self.rotation.AddY(v);
    public static void AddRotationZ(this Transform self, float v) => self.rotation = self.rotation.AddZ(v);
    public static void AddRotationW(this Transform self, float v) => self.rotation = self.rotation.AddW(v);

    /// <summary>
    /// 回転に x を加える
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    public static void AddEulerAngleX(this Transform self, float v) => self.eulerAngles = self.eulerAngles.AddX(v);
    public static void AddEulerAngleY(this Transform self, float v) => self.eulerAngles = self.eulerAngles.AddY(v);
    public static void AddEulerAngleZ(this Transform self, float v) => self.eulerAngles = self.eulerAngles.AddZ(v);
    public static void AddEulerAngle(this Transform self, float v) => self.eulerAngles = self.eulerAngles.Add(v);

    /// <summary>
    /// スケールをセットする
    /// </summary>
    /// <param name="self"></param>
    /// <param name="s"></param>
    public static void SetLocalScaleX(this Transform self, float v) => self.localScale = self.localScale.SetX(v);
    public static void SetLocalScaleY(this Transform self, float v) => self.localScale = self.localScale.SetY(v);
    public static void SetLocalScaleZ(this Transform self, float v) => self.localScale = self.localScale.SetZ(v);
    public static void SetLocalScale(this Transform self, float v) => self.localScale = self.localScale.Set(v);
}
