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
}
