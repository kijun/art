using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SetupLevel();
	}

    void SetupLevel() {
        StartCoroutine(PatternManager.Grid(duration: 60));
        //PatternManager.Swarm(duration: 20);
    }

	// Update is called once per frame
	void Update () {

	}
}


// probabaly there's a better name
public class PatternManager {
    // follow camera, move at speed, move with acceleration, constant, bezier, etc. but how?
    // perhaps too many args?
    // probably color and particle etc should be taken out as a separate parameter.
    // TODO maybe like a chain something returns where things should be drawn, and the renderer works on it, perhaps
    // points -> rendering
    // this refactor should probably happen later
    // TODO remove static
    public static IEnumerator Grid(
            float start=0,
            float duration=10,
            int dots=40,
            float speed= 6,
            float width=50,
            float fadeinDuration=5,
            float fadeoutDuration=10,
            float particleSize=0.1f,
            Direction startDirection=Direction.Center,
            Vector2 displacement = new Vector2(),
            LinearInterpolator locationInterpolator = null)
    {

        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time < endTime) {
            float progress = (Time.time - startTime) / duration;
            float angle = progress * speed;

            var coeff = new Dictionary<float, Complex>();
            coeff.Add(1, Complex.FromDegrees(angle));
            coeff.Add(-2, Complex.FromDegrees(-angle));
            coeff.Add(3, Complex.FromDegrees(-angle));
            Complex[] samples = DFT.GenerateSamples(coeff, dots);
            Vector2[] positions = new Vector2[dots];
            Vector2 center = ScreenUtil.ScreenLocationToWorldPosition(startDirection);

            for (int i = 0; i < dots; i++) {
                positions[i] = center + new Vector2(samples[i].real, samples[i].img);
            }

            Render(positions, ShapeType.Circle, particleSize);

            yield return null;
        }
    }

    public void Swarm(float start=0, float duration=10, int dots=40, float speed= 6, float width=50, float fadeinDuration=5, float fadeoutDuration=10, float dotsize=0.1f) {
    }

    // TODO remove static
    public static void Render(Vector2[] pos, ShapeType shape, float particleSize) {
        // TODO got to figure out object recycling
        // TODO smooth interpolator?
        if (shape == ShapeType.Circle) {
            foreach (Vector2 p in pos) {
                CircleProperty2 prop = new CircleProperty2(center:p);
                ResourceLoader.InstantiateCircle(prop);
            }
        }
        /*
        else if (shape == ShapeType.Line) {
            LineProperty prop = new LineProperty();
            var obj = ResourceLoader.InstantiateLine(prop);
        } else if (shape == ShapeType.Rect) {
            ResourceLoader.InstantiateRect();
            var obj = ResourceLoader.InstantiateLine(prop);
        }
        */
    }
}
