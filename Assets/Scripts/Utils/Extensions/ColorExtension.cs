using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorExtension {
    public static Color WithAlpha(this Color color, float alpha) {
        return new Color(color.r, color.g, color.b, Mathf.Min(alpha, 1));
    }

    public static bool RGBEquals(this Color color, Color other) {
        return color.r.Approx(other.r) && color.g.Approx(other.g) && color.b.Approx(other.b);
    }
}
