using UnityEngine;
using System.Collections;

public class ObjectFader : MonoBehaviour {

    public void FadeAndDestroy(float seconds) {
        StartCoroutine(DoFadeAndDestroy(seconds));
    }

    IEnumerator DoFadeAndDestroy(float fadeTime) {
        Material mainMat = GetComponent<Renderer>().material;
        // Derived from OVRScreenFade
        float elapsedTime = 0.0f;
        Color color = mainMat.color;
        while (elapsedTime < fadeTime)
        {
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
            color.a = 1.0f - Mathf.Clamp01(elapsedTime / fadeTime);
            mainMat.color = color;
        }

        Destroy(gameObject);
    }
}
