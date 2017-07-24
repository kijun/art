using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationField : MonoBehaviour {
    // Prefab
    public Animatable2 prefab;

    // Duration
    public float startTime;
    public float endTime;

    // Dimension
    public Range widthRange;
    public Range heightRange;
    public float thicknessRange;

    // Velocity
    public Range angularVelocity;

    // Creation
    public Range timeBetweenActivation;
    public float scaleVelocity;

	// Use this for initialization
	void Start () {
//        StartCoroutine(Run());
	}

    /*
    IEnumerator Run() {
        yield return new WaitForSeconds(startTime);
        while (endTime < float.Epsilon || Time.time < endTime) {
            StartCoroutine(CreateAndMoveObject());
            yield return new WaitForSeconds(timeBetweenActivation.RandomValue());
        }
    }

    IEnumerator CreateAndMoveObject() {
        // generate object -animatable?
        //
        //

        var width = widthRange.RandomValue();
        var height = heightRange.RandomValue();

        var animatable = CreateObject(transform.position, rotationRange.minimum, width, height);
        animatable.pivot = pivot;

        animatable.angularVelocity = angularVelocity.RandomValue();
        if (scaleVelocity.sqrMagnitude > float.Epsilon) {
            animatable.scaleVelocity = scaleVelocity;
            animatable.nonNegativeScale = true;
        }
        yield return true;
    }

    Animatable2 CreateObject(Vector2 position, float rotation, float width, float height) {
        var obj = Animatable2.Instantiate(prefab, position, Quaternion.Euler(0, 0, rotation));
        obj.localScale = new Vector2(width, height);
        return obj;
    }
    */
}

