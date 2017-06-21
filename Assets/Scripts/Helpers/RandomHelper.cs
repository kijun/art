using UnityEngine;
using System.Collections.Generic;

public static class RandomHelper {
    public static float Between(float a, float b) {
        return a + ((b - a) * Random.value);
    }

    public static Vector2 RandomVector2(Vector2 v1, Vector2 v2) {
        return new Vector2(Random.Range(v1.x, v2.x), Random.Range(v1.y, v2.y));
    }

    public static List<float> Points(float from, float to, int cnt) {

        var points = new List<float>();

        for (int i=0; i<cnt; i++) {
            points.Add((to-from) * Random.value + from);
        }

        points.Sort();

        return points;
    }

    public static List<int> Points(int from, int to, int cnt, bool unique = true) {

        var uniquePoints = new HashSet<int>();

        while (uniquePoints.Count < cnt) {
            uniquePoints.Add((int)((to-from) * Random.value) + from);
        }

        var points = new List<int>();

        foreach (var p in uniquePoints) {
            points.Add(p);
        }

        points.Sort();

        return points;
    }

    public static float Pick(float[] list) {
        return list[Random.Range(0, list.Length)];
    }

    public static T Pick<T>(params T[] choices) {
        return choices[Random.Range(0, choices.Length)];
    }
}

