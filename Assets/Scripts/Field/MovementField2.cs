using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementField2 : BaseField {
    // 0 is up, 180 is down
    public Vector2 velocity;
    public Range heightRange;
    public Range widthRange;
    public Range allowedAngle = new Range(135, 215);
    public Range timeBetweenActivation;
    public Vector2 origin = new Vector2(0, 0);

    void Start() {
        StartCoroutine(Run());
    }

    IEnumerator Run() {
        yield return new WaitForSeconds(startTime);
        while (endTime < float.Epsilon || Time.time < endTime) {
            var target = StationaryObject();
            if (target) {
                StartCoroutine(MoveObject(target));
            }
            yield return new WaitForSeconds(timeBetweenActivation.RandomValue());
        }
    }

    Animatable StationaryObject() {
        Animatable target = targets[0];
        var width = widthRange.RandomValue();
        var height = heightRange.RandomValue();

        // Determine position
        var entryAngle = allowedAngle.RandomValue();

        var distanceFromOrigin = cameraDiameter / 2 + Mathf.Sqrt(width * width + height * height) / 2;

        var objectPos = Quaternion.Euler(0, 0, entryAngle) * Vector2.up * distanceFromOrigin;

        var animatable = GameObject.Instantiate<Animatable>(target, objectPos, Quaternion.identity);

        animatable.localScale = new Vector2(width, height);
        return animatable;
    }

    IEnumerator MoveObject(Animatable target) {
        target.velocity = velocity;

        var waitTime = (cameraDiameter + target.localScale.magnitude) / velocity.magnitude;

        Debug.Log("Diameter" + cameraDiameter);
        Debug.Log(waitTime);

        yield return new WaitForSeconds(waitTime);

        target.StopMovement();
    }

    float cameraDiameter {
        get {
            var inGameHeight = Camera.main.orthographicSize * 2;
            var inGameWidth = Screen.width / Screen.height * inGameHeight;

            var cameraDiameter = Mathf.Sqrt(inGameHeight * inGameHeight + inGameWidth * inGameWidth);

            return cameraDiameter;
        }
    }
}

