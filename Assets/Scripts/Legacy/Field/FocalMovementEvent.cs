using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocalMovementEvent : BaseEvent {
    public Vector2 focus;
    public bool diverge = false;
    public float speed;
    public float angularSpeed;
    public bool rotationEnabled = false;
    public bool rotationMirrored = true;
    bool started;

    void Update() {
        if (!started && Time.time > startTime) {
            foreach (var target in targets) {
                target.velocity = (focus - (Vector2)target.transform.position).normalized * speed;
                if (diverge) target.velocity *= -1;
                if (rotationEnabled) {
                    target.angularVelocity = angularSpeed;
                    if (rotationMirrored && target.position.x < 0) {
                        target.angularVelocity *= -1;
                    }
                }
            }
            started = true;
        }
    }
}
