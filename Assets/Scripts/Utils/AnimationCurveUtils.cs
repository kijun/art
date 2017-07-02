using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationCurveUtil {
    public static AnimationCurve FromPairs(params float pairs) {
        new AnimationCurve(KeyframeHelper.CreateKeyframes(pairs));
    }
}
