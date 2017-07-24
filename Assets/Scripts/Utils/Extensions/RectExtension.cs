using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectExtensions {
    public static Vector2 RandomPosition(this Rect rect) {
        return new Vector2(Random.Range(rect.xMin, rect.xMax), Random.Range(rect.yMin, rect.yMax));
    }
}

public static class RectHelper {
    public static Rect RectFromCenterAndSize(Vector2 center, Vector2 size) {
        return new Rect(center - size/2, size);
    }
}
