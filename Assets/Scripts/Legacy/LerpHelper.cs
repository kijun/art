using UnityEngine;
using System.Collections;

public delegate void LerpDelegate(float progress);

public static class LerpHelper {
    public static IEnumerator Lerp(LerpDelegate fadeFunc, float duration) {
        float elapsedTime = 0.0f;
        while (elapsedTime < duration) {
            yield return null;
            elapsedTime += Time.deltaTime;
            fadeFunc(elapsedTime/duration);
        }
        yield return null;
    }
}
