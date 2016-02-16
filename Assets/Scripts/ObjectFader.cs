using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectFader : MonoBehaviour {

    public void FadeAndDestroy(float seconds) {
        StartCoroutine(DoFadeAndDestroy(seconds));
    }

    IEnumerator DoFadeAndDestroy(float fadeTime) {
        var materials = new List<Material>();

        var selfRenderer = GetComponent<Renderer>();
        if (selfRenderer != null) {
            materials.Add(selfRenderer.material);
        }

        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
        foreach (var r in childRenderers) {
            materials.Add(r.material);
        }

        if (materials.Count == 0) {
            yield return null;
        } else {
            Material mainMat = materials[0];
            // Derived from OVRScreenFade
            float elapsedTime = 0.0f;
            // obviously doesn't work if materials have different color
            Color color = mainMat.color;
            while (elapsedTime < fadeTime) {
                yield return new WaitForEndOfFrame();
                foreach (var m in materials) {
                    elapsedTime += Time.deltaTime;
                    color.a = 1.0f - Mathf.Clamp01(elapsedTime / fadeTime);
                    m.color = color;
                }
            }
        }

        Destroy(gameObject);
    }
}
