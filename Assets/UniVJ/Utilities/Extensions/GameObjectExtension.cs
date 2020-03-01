using UnityEngine;

public static class GameObjectExtension
{
    public static void SetLayerRecursively(this GameObject self, int layer)
    {
        self.layer = layer;
        foreach(Transform t in self.transform)
        {
            SetLayerRecursively(t.gameObject, layer);
        }
    }
}
