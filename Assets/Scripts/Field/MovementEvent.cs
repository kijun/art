using UnityEngine;

public class MovementEvent : BaseEvent {
    public Vector2 velocity;
    public bool velocityEnabled = true;

    public float rotation;
    public bool rotationEnabled = true;

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


