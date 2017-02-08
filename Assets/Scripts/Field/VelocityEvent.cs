using UnityEngine;

public class VelocityEvent : BaseEvent {
    public Vector2 velocity;
    bool started;

    void Update() {
        if (!started && Time.time > startTime) {
            foreach (var target in targets) {
                target.velocity = velocity;
            }
            started = true;

        }
    }
}

