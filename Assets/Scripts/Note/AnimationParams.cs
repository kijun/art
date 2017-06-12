using UnityEngine;

/*
 * 0 - 1 (fade in) - 0 (fade out)
 */

[System.Serializable]
public struct AnimationParams {
    public float startOpacity;
    public float middleOpacity;
    public float endOpacity;

    public float startTime;
    public float e;

    /*
    public float opacity(float timeFromInit) {
        bool fadeIn = float.Equals(fadeInTime, 0);
        return (fadeOutTime - fadeInTime);
    }
    */
}

/*
public struct Color

enum FadeAnimationPhase {
    FadeIn,
    FadeOut,

}
*/
