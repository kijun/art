using UnityEngine;

public class VelocityEvent : BaseEvent {
    public Vector2 velocity;
    public float angularVelocity;
    bool started;

    void Update() {
        if (!started && Time.time > startTime) {
            foreach (var target in targets) {
                target.velocity = velocity;
                target.angularVelocity = angularVelocity;
            }
            started = true;

        }
    }
}

