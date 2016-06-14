using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* swirling circle */
/* swirls and then becomes one, and then diverges again */
public class Pattern1 : MonoBehaviour {

    public int numberOfCircles = 10;
    public Range radius = new Range(1, 10);
    public float collapseDuration = 5f;
    public float delayBetweenCircle = 0.2f;
    List<CircleProperty> circles = new List<CircleProperty>();
    List<Vector2> startPos = new List<Vector2>();

	void Start () {
        for (int i=0; i<numberOfCircles; i++) {
            var angle = i * 360f / numberOfCircles;
            var prop = new CircleProperty2(
                    diameter: 0.2f,
                    center: radius.maximum  * InterpolationUtil.UnitVectorWithAngle(angle)
            );

            var circle = ResourceLoader.InstantiateCircle(prop);
            circle.OnUpdate();

            startPos.Add(prop.center);
            circles.Add(circle);
        }
        StartCoroutine(Run());
	}

    IEnumerator Run() {
        yield return new WaitForSeconds(1);
        float startTime = Time.time;
        while (true) {
            for (int i = 0; i<numberOfCircles; i++) {
                var circle = circles[i];
                var progress = (Time.time - startTime - delayBetweenCircle*i) / collapseDuration;
                circle.center = InterpolationUtil.Bezier(startPos[i], new Vector2(i-numberOfCircles/2f, i/2f), progress);
            }
            yield return null;
        }
    }

	void Update () {

	}
}
