using UnityEngine;

public static class QuaternionExtension
{
    public static Quaternion AddX(this Quaternion self, float x) => new Quaternion(self.x + x, self.y, self.z, self.w);
    public static Quaternion AddY(this Quaternion self, float y) => new Quaternion(self.x, self.y + y, self.z, self.w);
    public static Quaternion AddZ(this Quaternion self, float z) => new Quaternion(self.x, self.y, self.z + z, self.w);
    public static Quaternion AddW(this Quaternion self, float w) => new Quaternion(self.x, self.y, self.z, self.w + w);
}