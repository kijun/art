using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Creates objects outside camera, and pushes it near origin
 */
public class RotationMovementField : BaseField {
    // 0 is up, 180 is down
    public Vector2 velocity;
    public Range heightRange;
    public Range widthRange;
    public Range allowedAngle = new Range(135, 215);
    public Range timeBetweenActivation;
    public Vector2 centerRange;
    public Range objPerActivation = new Range(1, 1);
    public Vector2 scaleVelocity = Vector2.zero;

    public bool turn = false;
    public float turnDuration = 0;

    public float randomPause = 0;

    void Start() {
        StartCoroutine(Run());
    }

    IEnumerator Run() {
        yield return new WaitForSeconds(startTime);
        while (endTime < float.Epsilon || Time.time < endTime) {
            var numObj = Mathf.RoundToInt(objPerActivation.RandomValue());
            for (int i = 0; i < numObj; i++) {
                StartCoroutine(CreateAndMoveObject());
            }
            yield return new WaitForSeconds(timeBetweenActivation.RandomValue());
        }
    }

    IEnumerator CreateAndMoveObject() {
        /**
         * Create Object
         */
        Animatable prefab = targets[0];
        var width = widthRange.RandomValue();
        var height = heightRange.RandomValue();

        // Determine position
        var entryAngle = allowedAngle.RandomValue();

        var distanceFromOrigin = cameraDiameter / 2 + Mathf.Sqrt(width * width + height * height) / 2;

        //var distanceFromOrigin = cameraDiameter / 2 + Mathf.Sqrt(width * width + height * height) / 2;


        var objectPos = Quaternion.Euler(0, 0, entryAngle) * (Vector2.up * distanceFromOrigin + new Vector2(centerRange.x*Random.value - centerRange.x/2, centerRange.y * Random.value - centerRange.y/2));

        var rotation = Quaternion.Euler(0, 0, entryAngle);

        var target = GameObject.Instantiate<Animatable>(prefab, objectPos, rotation);

        var c = target.GetComponent<SpriteRenderer>().material.color;
        c.a = Random.Range(0.3f, 0.8f);
        target.GetComponent<SpriteRenderer>().material.color = c;


        target.localScale = new Vector2(width, height);

        /**
         * Scale Object
         */


        if (scaleVelocity.sqrMagnitude > float.Epsilon) {
            target.scaleVelocity = scaleVelocity;
            target.nonNegativeScale = true;
        }

        /**
         * Move Object
         */

        target.velocity = rotation * velocity;

        //var waitTime = (cameraDiameter + target.localScale.magnitude) / velocity.magnitude;
        var waitTime = (distanceFromOrigin * 2) / velocity.magnitude;


        if (turn) {
            yield return new WaitForSeconds(waitTime/2);

            // Rotation
            var scaleGoal = new Vector2(target.localScale.y, target.localScale.x);
            var scaleVelocity = (scaleGoal - target.localScale) / turnDuration;
            target.StopMovement();
            target.scaleVelocity = scaleVelocity;
            yield return new WaitForSeconds(turnDuration);
            target.scaleVelocity = Vector2.zero;

            target.velocity = Random.value > 0.5 ? Quaternion.Euler(0, 0, 90) * velocity : Quaternion.Euler(0, 0, -90) * velocity;
            // just make sure it exits
            yield return new WaitForSeconds(waitTime);
        } else {
            if (randomPause > float.Epsilon) {
                var randomTime = waitTime * 0.5f;
                yield return new WaitForSeconds(randomTime);

                var scaleV = new Vector2(Mathf.Abs(target.localScale.x-1), Mathf.Abs(target.localScale.y-1));
                target.scaleVelocity = -1 * scaleV / randomPause;
                target.velocity = velocity.normalized * target.scaleVelocity.magnitude;
                yield return new WaitForSeconds(randomPause);
                target.scaleVelocity = scaleV / randomPause;
                yield return new WaitForSeconds(randomPause);

                target.scaleVelocity = Vector2.zero;
                target.velocity = velocity;
                yield return new WaitForSeconds(waitTime - randomTime);
            } else {
                yield return new WaitForSeconds(waitTime);
            }
        }

        target.StopMovement();
        Destroy(target.gameObject);
    }

    float cameraDiameter {
        get {
            var inGameHeight = Camera.main.orthographicSize * 2;
            var inGameWidth = (float)Screen.width / (float)Screen.height * inGameHeight;

            var cameraDiameter = Mathf.Sqrt(inGameHeight * inGameHeight + inGameWidth * inGameWidth);

            return cameraDiameter;
        }
    }
}

