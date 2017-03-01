using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Creates
 */
public class CreationField : BaseField {

    public Range timeBetweenCreation = new Range(1, 1);
    public int objectsPerCreationEvent = 1;

    public Vector2 minObjectSize = Vector2.one;
    public Vector2 maxObjectSize = Vector2.one;
    public Range objectAngle = new Range(0, 0);

    void Start() {
        StartCoroutine(Run());
    }

    IEnumerator Run() {
        yield return new WaitForSeconds(startTime);
        while (endTime < float.Epsilon || Time.time < endTime) {
            for (int i = 0; i < objectsPerCreationEvent; i++) {
                var target = CreateObject();
            }
            /*
            if (target) {
                StartCoroutine(MoveObject(target));
            }
            */
            yield return new WaitForSeconds(timeBetweenCreation.RandomValue());
        }
    }

    Animatable CreateObject() {
        Animatable target = targets[0];

        // Determine position
        var entryAngle = objectAngle.RandomValue();

        var objectPos = CameraHelper.WorldRect.RandomPosition();

        var animatable = GameObject.Instantiate<Animatable>(target, objectPos, Quaternion.Euler(0, 0, entryAngle));

        animatable.localScale = RandomHelper.RandomVector2(minObjectSize, maxObjectSize);
        return animatable;
    }
}
