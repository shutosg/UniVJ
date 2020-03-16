using UnityEngine;

public static class VectorExtension
{
    public static Vector3 AddX(this Vector3 self, float v) => new Vector3(self.x + v, self.y, self.z);
    public static Vector3 AddY(this Vector3 self, float v) => new Vector3(self.x, self.y + v, self.z);
    public static Vector3 AddZ(this Vector3 self, float v) => new Vector3(self.x, self.y, self.z + v);
    public static Vector3 Add(this Vector3 self, float v) => new Vector3(self.x + v, self.y + v, self.z + v);

    public static Vector3 SetX(this Vector3 self, float v) => new Vector3(v, self.y, self.z);
    public static Vector3 SetY(this Vector3 self, float v) => new Vector3(self.x, v, self.z);
    public static Vector3 SetZ(this Vector3 self, float v) => new Vector3(self.x, self.y, v);
    public static Vector3 Set(this Vector3 self, float v) => new Vector3(v, v, v);
}