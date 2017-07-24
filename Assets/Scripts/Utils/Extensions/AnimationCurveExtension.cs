using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationCurveExtensions {
    public static bool IsDone(this AnimationCurve curve, float currentTime){
        if (curve.length == 0) return true;
        return curve.keys[curve.length-1].time < currentTime;
    }
}
