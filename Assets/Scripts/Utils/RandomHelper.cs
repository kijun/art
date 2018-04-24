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

    public static Vector2 RandomVector2(float xmin, float xmax, float ymin, float ymax) {
        return new Vector2(Random.Range(xmin, xmax), Random.Range(ymin, ymax));
    }

    public static float[] NormalizedWidths(int cnt) {
        var widths = new float[cnt];
        var sum = 0.0f;
        for (int i = 0; i < cnt; i++) {
            widths[i] = Random.value;
            sum += widths[i];
        }

        for (int i = 0; i < cnt; i++) {
            widths[i] /= sum;
        }

        return widths;
    }

    public static float[] NormalizedWidths(int cnt, out float[] offsets) {
        var widths = new float[cnt];
        offsets = new float[cnt];
        var sum = 0.0f;
        var currOffset = 0.0f;
        for (int i = 0; i < cnt; i++) {
            widths[i] = Random.value;
            sum += widths[i];
        }

        for (int i = 0; i < cnt; i++) {
            offsets[i] = currOffset;
            widths[i] /= sum;
            currOffset += widths[i];
        }

        return widths;
    }
}

