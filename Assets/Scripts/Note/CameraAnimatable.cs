using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimatable : Animatable2 {
    public float orthographicSizeVelocity;

    public void Update() {
        if (Mathf.Abs(orthographicSizeVelocity) > float.Epsilon) {
            Camera.main.orthographicSize += orthographicSizeVelocity * Time.deltaTime;
        }
    }
}
