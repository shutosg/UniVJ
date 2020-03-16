using UnityEngine;

public static class GameObjectExtension
{
    /// <summary>
    /// レイヤーを自身と子に再帰的に設定していく
    /// </summary>
    /// <param name="self"></param>
    /// <param name="layer"></param>
    /// <param name="ignoreSelf"></param>
    public static void SetLayerRecursively(this GameObject self, int layer, bool ignoreSelf = false)
    {
        if (!ignoreSelf) self.layer = layer;
        foreach (Transform t in self.transform)
        {
            SetLayerRecursively(t.gameObject, layer);
        }
    }
}