using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformEvent : BaseEvent {
    public Vector2 position;
    public bool applyPosition = true;
    public float rotation;
    public bool applyRotation = true;

    void Start() {
        StartCoroutine(Run());
    }

    IEnumerator Run() {
        yield return new WaitForSeconds(startTime);
        foreach (var target in targets) {
            target.position = position;
            target.rotation = rotation;
        }
    }
}
