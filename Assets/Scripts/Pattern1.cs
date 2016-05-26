using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Interpolator {
    // converts time to ratio and also gives . basic lerp and stuff good.
    public static float Lerp(Range range, float progress) {
        progress = Mathf.Clamp(progress, 0f, 1f);
        return progress * (range.maximum - range.minimum) + range.minimum;
    }

    public static Vector2 UnitVectorWithAngle(float angleInDegrees) {
        var angleInRadian = angleInDegrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angleInRadian), Mathf.Sin(angleInRadian));
    }

    public static Vector2 Lerp(Vector2 start, Vector2 end, float progress) {
        return Vector3.Lerp(start, end, progress);
    }

    public static Vector2 Bezier(Vector2 start, Vector2 end, float progress) {
        return end;
    }
}

/* swirling circle */
/* swirls and then becomes one, and then diverges again */
public class Pattern1 : MonoBehaviour {

    public int numberOfCircles = 10;
    public Range radius = new Range(1, 5);
    public float collapseDuration = 3f;
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
        float startTime = Time.time;
        while (true) {
            for (int i = 0; i<numberOfCircles; i++) {
                var circle = circles[i];
                var progress = (Time.time - startTime) / collapseDuration;
                circle.center = Interpolator.Lerp(startPos[i], Vector2.zero, progress);
            }
            yield return null;
        }
    }

	void Update () {

	}
}

public class CircleProperty2 {
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
            float           borderThickness = 0,
            float           dashLength = 0.05f,
            float           gapLength = 0.05f
    ) {
        this.diameter = diameter;
        this.center = center;
        this.borderStyle = borderStyle;
        this.borderPosition = borderPosition;
        this.borderThickness = borderThickness;
        this.dashLength = dashLength;
        this.gapLength = gapLength;
    }
}

// model
// view
