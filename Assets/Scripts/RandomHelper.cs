using UnityEngine;

public static class RandomHelper {
    public static float Between(float a, float b) {
        return a + ((b - a) * Random.value);
    }

    public static Vector2 RandomVector2(Vector2 v1, Vector2 v2) {
        return new Vector2(Random.Range(v1.x, v2.x), Random.Range(v1.y, v2.y));
    }
}

