using UnityEngine;

public static class TransformExtension
{
    public static void AddPositionX(this Transform self, float v) => self.position = self.position.AddX(v);
    public static void AddPositionY(this Transform self, float v) => self.position = self.position.AddY(v);
    public static void AddPositionZ(this Transform self, float v) => self.position = self.position.AddZ(v);

    public static void AddRotationX(this Transform self, float v) => self.rotation = self.rotation.AddX(v);
    public static void AddRotationY(this Transform self, float v) => self.rotation = self.rotation.AddY(v);
    public static void AddRotationZ(this Transform self, float v) => self.rotation = self.rotation.AddZ(v);
    public static void AddRotationW(this Transform self, float v) => self.rotation = self.rotation.AddW(v);

    public static void AddEulerAngleX(this Transform self, float v) => self.eulerAngles = self.eulerAngles.AddX(v);
    public static void AddEulerAngleY(this Transform self, float v) => self.eulerAngles = self.eulerAngles.AddY(v);
    public static void AddEulerAngleZ(this Transform self, float v) => self.eulerAngles = self.eulerAngles.AddZ(v);
    public static void AddEulerAngle(this Transform self, float v) => self.eulerAngles = self.eulerAngles.Add(v);
    public static void SetLocalEulerAngleX(this Transform self, float v) => self.localEulerAngles = self.localEulerAngles.SetX(v);
    public static void SetLocalEulerAngleY(this Transform self, float v) => self.localEulerAngles = self.localEulerAngles.SetY(v);
    public static void SetLocalEulerAngleZ(this Transform self, float v) => self.localEulerAngles = self.localEulerAngles.SetZ(v);
    public static void SetLocalEulerAngle(this Transform self, float v) => self.localEulerAngles = self.localEulerAngles.Set(v);

    public static void SetLocalScaleX(this Transform self, float v) => self.localScale = self.localScale.SetX(v);
    public static void SetLocalScaleY(this Transform self, float v) => self.localScale = self.localScale.SetY(v);
    public static void SetLocalScaleZ(this Transform self, float v) => self.localScale = self.localScale.SetZ(v);
    public static void SetLocalScale(this Transform self, float v) => self.localScale = self.localScale.Set(v);
}