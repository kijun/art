using UnityEngine;

public class RotationEvent : BaseEvent {
    public float rotation;
    bool started;

    void Update() {
        if (!started && Time.time > startTime) {
            foreach (var target in targets) {
                target.rotation = rotation;
            }
            started = true;

        }
    }
}

