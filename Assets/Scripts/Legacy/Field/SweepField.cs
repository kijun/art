using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweepField : MonoBehaviour {
    public float startTime;
    public float endTime;
    public Range widthRange;
    public Range heightRange;
    public Vector2 pivot;
    public Range rotationRange = new Range(0, 180);
    public Range angularVelocity;
    public Range timeBetweenActivation;
    public Range objPerActivation = new Range(1, 1);
    public Vector2 scaleVelocity = Vector2.zero;
    public Animatable2 prefab;

	// Use this for initialization
	void Start () {
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
}
