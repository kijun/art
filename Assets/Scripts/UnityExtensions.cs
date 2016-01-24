using UnityEngine;

public static class UnityExtensions {
    public static Vector3 SwapX(this Vector3 v, float val) {
        v.x = val;
        return v;
    }

    public static Vector3 SwapY(this Vector3 v, float val) {
        v.y = val;
        return v;
    }

    public static Vector3 SwapZ(this Vector3 v, float val) {
        v.z = val;
        return v;
    }

    public static Vector3 IncrX(this Vector3 v, float val) {
        v.x += val;
        return v;
    }

    public static Vector3 IncrY(this Vector3 v, float val) {
        v.y += val;
        return v;
    }

    public static Vector3 IncrZ(this Vector3 v, float val) {
        v.z += val;
        return v;
    }

    public static Vector2 RandomPoint(this Bounds b) {
        var center = b.center;
        var extents = b.extents;
        return new Vector2(Random.Range(center.x - extents.x, center.x + extents.x),
                           Random.Range(center.y - extents.y, center.y + extents.y));
    }
}
