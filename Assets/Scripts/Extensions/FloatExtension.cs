using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatExtension {
    public static bool IsNonZero(this float f) {
        return Mathf.Abs(f) > float.Epsilon;
    }

    public static bool IsZero(this float f) {
        return f < float.Epsilon && f > -float.Epsilon;
    }

    public static bool Approx(this float f1, float f2) {
        return Mathf.Approximately(f1, f2);
    }
}
