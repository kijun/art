using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementField : BaseField {
    public Vector2 velocity;
    public float movementDuration;
    public float timeBetweenAction;
    public bool randomizeOrder;
    public Vector2 origin = new Vector2(0, 0);

    int numObjInMovement = 0;

    void Start() {
        StartCoroutine(Run());
    }

    IEnumerator Run() {
        yield return new WaitForSeconds(startTime);
        while (Time.time < endTime) {
            var target = StationaryObject();
            if (target) {
                StartCoroutine(MoveObject(target));
            }
            yield return new WaitForSeconds(timeBetweenAction);
        }
    }

    Animatable StationaryObject() {
        if (numObjInMovement < targets.Length) {
            while (true) {
                Animatable target = targets[Random.Range(0, targets.Length)];
                if (target.velocity.sqrMagnitude < float.Epsilon) {
                    return target;
                }
            }
        }
        return null;
    }

    IEnumerator MoveObject(Animatable target) {
        bool verticalMovement = velocity.x > velocity.y;
        if (verticalMovement) {
            target.velocity = target.position.x < origin.x ? velocity : -1 * velocity;
//            target.scaleVelocity = Vector2.one;
        } else {
            target.velocity = target.position.y < origin.y ? velocity : -1 * velocity;
        }
        numObjInMovement++;
        yield return new WaitForSeconds(movementDuration);

        target.StopMovement();
        numObjInMovement--;
    }
}
