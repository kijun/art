using UnityEngine;
using System.Collections;


delegate IEnumerator ActionDelegate(float duration);


public class ExampleLevel1 : MonoBehaviour {

    public CircleRenderer perimeter;

	// Use this for initialization
	void Start () {
        StartCoroutine(Run());
	}

	// Update is called once per frame
	void Update () {

	}

    IEnumerator Run () {
        //yield return StartCoroutine(fadeOut);
        yield return StartCoroutine(ChangeCircleColor());
        yield return StartCoroutine(EnlargeCircle());
        // fade in to a large circle
        // fade in music
        // the circle expands and reveals inner
        // show a word
        // initate progress bar
        // move structure 1
        // move structure 2
        // when complete
        // slow down time
        // fade out
        yield return null;
    }

    IEnumerator ChangeCircleColor() {
        float timeout = Time.time + 10;
        yield return null;
    }

    IEnumerator EnlargeCircle() {
        float timeout = Time.time + 10;
        var objDiameter = perimeter.property.diameter;
        while (Time.time < timeout) {
            // TODO fuck
            var circle = perimeter.property.Clone() as CircleProperty;
            circle.innerCircleDiameter += Time.deltaTime*3;
            circle.innerCircleDiameter = Mathf.Max(circle.innerCircleDiameter, objDiameter-0.1f);
            perimeter.property = circle;
            yield return null;
        }
    }
}
