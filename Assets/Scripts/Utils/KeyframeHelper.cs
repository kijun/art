using UnityEngine;

public static class KeyframeHelper {
    public static Keyframe[] CreateKeyframes(params float[] pairs) {
        if (pairs.Length % 2 == 1) {
            throw new System.Exception("Input must be in pairs");
        }

        var keyframeArray = new Keyframe[pairs.Length/2];
        for (int i = 0; i < pairs.Length / 2; i++) {
            keyframeArray[i] = new Keyframe(pairs[2*i], pairs[2*i + 1]);
        }

        return keyframeArray;
    }
}
