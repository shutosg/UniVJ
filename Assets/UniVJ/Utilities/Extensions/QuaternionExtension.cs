using UnityEngine;

public static class QuaternionExtension
{
    /// <summary>
    /// 自身に x を加えた新しい Quaternion を返す
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    public static Quaternion AddX(this Quaternion self, float x) => new Quaternion(self.x + x, self.y, self.z, self.w);
    public static Quaternion AddY(this Quaternion self, float y) => new Quaternion(self.x, self.y + y, self.z, self.w);
    public static Quaternion AddZ(this Quaternion self, float z) => new Quaternion(self.x, self.y, self.z + z, self.w);
    public static Quaternion AddW(this Quaternion self, float w) => new Quaternion(self.x, self.y, self.z, self.w + w);
}
