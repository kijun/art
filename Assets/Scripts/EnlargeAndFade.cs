using UnityEngine;
using System.Collections;

public class EnlargeAndFade : MonoBehaviour {
    public float maxSize = 5f;
    public float enlargeDuration = 5f;
    Vector3 originalSize;
    float startTime;

    void Start() {
        originalSize = transform.localScale;
    }

    void Update() {
        if (startTime < float.Epsilon) {
            startTime = Time.time;
        }
        float timeFromStart = Time.time - startTime;
        if (timeFromStart > enlargeDuration) return;
        transform.localScale = Vector3.Lerp(originalSize, originalSize*5f, timeFromStart/enlargeDuration);
    }
}
