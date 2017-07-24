using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeField : BaseField {
    public float cadence;
    /*
    public Vector2 minScale = Vector2.one;
    public Vector2 maxScale = new Vector2(2, 2);
    */
    public Vector2 scaleVelocity = Vector2.one;

    void Start() {
        // after startTime,
        foreach (var target in targets) {
            StartCoroutine(Rescale(target));
        }
    }

    // TODO
    // Add scale velocity to target
    IEnumerator Rescale(Animatable target) {
        yield return new WaitForSeconds(startTime);
        bool scaleUp = true;
        while (Time.time < endTime) {
            var reverseAt = Time.time + cadence;
            target.scaleVelocity = scaleUp ? scaleVelocity : -1 * scaleVelocity;
            while (Time.time < reverseAt) {
                yield return null;
            }
            scaleUp = !scaleUp;
        }
    }
}
