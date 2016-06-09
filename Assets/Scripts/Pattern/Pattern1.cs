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
                    center: radius.maximum  * Interpolator.UnitVectorWithAngle(angle)
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
                circle.center = Interpolator.Bezier(startPos[i], new Vector2(i-numberOfCircles/2f, i/2f), progress);
            }
            yield return null;
        }
    }

	void Update () {

	}
}

public struct CircleProperty2 {
    public Vector2 center;
    public float diameter;
    public Color color;

    // border
    public BorderStyle borderStyle;
    public BorderPosition borderPosition;
    public Color borderColor;
    public float borderThickness;
    public float dashLength;
    public float gapLength;

    public CircleProperty2(
            float           diameter = 1,
            Vector2         center = new Vector2(),
            Color           color = new Color(),
            BorderStyle     borderStyle = BorderStyle.None,
            BorderPosition  borderPosition = BorderPosition.Center,
            Color           borderColor = new Color(),
            float           borderThickness = 0,
            float           dashLength = 0.05f,
            float           gapLength = 0.05f
    ) {
        this.diameter = diameter;
        this.center = center;
        this.color = color;
        this.borderStyle = borderStyle;
        this.borderPosition = borderPosition;
        this.borderThickness = borderThickness;
        this.borderColor = color;
        this.dashLength = dashLength;
        this.gapLength = gapLength;
    }
}

// model
// view
