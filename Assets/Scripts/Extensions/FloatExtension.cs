using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatExtension {
    public static bool IsNonZero(this float f) {
        return Mathf.Abs(f) > float.Epsilon;
    }
}
