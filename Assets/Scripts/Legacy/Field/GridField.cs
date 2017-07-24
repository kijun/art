using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Creates objects outside camera, and pushes it near origin, with rotation
 */
public class GridField : BaseField {
    // 0 is up, 180 is down
    public Vector2 velocity;
    public Range heightRange;
    public Range widthRange;
    public Range allowedAngle = new Range(135, 215);
    public Range timeBetweenActivation;
    public Vector2 centerRange;
    public Range objPerActivation = new Range(1, 1);
    public Vector2 scaleVelocity = Vector2.zero;
    public Range angularVelocityRange;

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

        target.angularVelocity = angularVelocityRange.RandomValue();

        yield return new WaitForSeconds(waitTime);

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

