using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorUtil {
    public static Color RandomColor(Range r, Range g, Range b, Range a) {
        return new Color(r.RandomValue(), g.RandomValue(), b.RandomValue(), a.RandomValue());
    }

    public static Color RandomColor() {
        var range = new Range(0, 1);
        return RandomColor(range, range, range, new Range(1, 1));
    }
}

