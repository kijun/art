using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementWithTurningField : BaseField {
    public Vector2 velocity;
    public float movementDuration;
    public float timeBetweenAction;
    public float rotationDuration = 1;
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
                if (target.stationary) return target;
            }
        }
        return null;
    }

    IEnumerator MoveObject(Animatable target) {
        // Movement Part 1
        bool verticalMovement = velocity.x > velocity.y;
        Vector2 velocityPart1;
        if (verticalMovement) {
            velocityPart1 = target.position.x < origin.x ? velocity : -1 * velocity;
        } else {
            velocityPart1 = target.position.y < origin.y ? velocity : -1 * velocity;
        }

        target.velocity = velocityPart1;
        numObjInMovement++;
        yield return new WaitForSeconds(movementDuration/2);

        // Rotation
        var scaleGoal = new Vector2(target.localScale.y, target.localScale.x);
        var scaleVelocity = (scaleGoal - target.localScale) / rotationDuration;
        Debug.Log(scaleGoal + " " + scaleVelocity);
        target.StopMovement();
        target.scaleVelocity = scaleVelocity;
        yield return new WaitForSeconds(rotationDuration);

        // Movement Part 2
        target.StopMovement();
        float velocityRotation = 90;
        if (Random.value > 0.5) velocityRotation *= -1;
        target.velocity = Quaternion.Euler(0, 0, velocityRotation) * velocityPart1;
        yield return new WaitForSeconds(movementDuration/2);

        // Stop Movement
        target.StopMovement();
        numObjInMovement--;
    }
}

