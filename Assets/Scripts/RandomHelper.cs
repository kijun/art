using UnityEngine;

public static class RandomHelper {
    public static float Between(float a, float b) {
        return a + ((b - a) * Random.value);
    }
}

