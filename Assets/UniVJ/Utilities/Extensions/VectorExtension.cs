using UnityEngine;

public static class VectorExtension
{
    /// <summary>
    /// 自身に x を加えた新しい Vector3 を返す
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    public static Vector3 AddX(this Vector3 self, float x) => new Vector3(self.x + x, self.y, self.z);
    public static Vector3 AddY(this Vector3 self, float y) => new Vector3(self.x, self.y + y, self.z);
    public static Vector3 AddZ(this Vector3 self, float z) => new Vector3(self.x, self.y, self.z + z);
}
