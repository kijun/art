using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationCurveUtils {
    public static AnimationCurve FromPairs(params float[] pairs) {
        return new AnimationCurve(KeyframeHelper.CreateKeyframes(pairs));
    }
}
