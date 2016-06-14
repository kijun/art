using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SetupLevel();
	}

    void SetupLevel() {
        PatternManager.Grid(duration: 60);
        PatternManager.Swarm(duration: 20);
    }

	// Update is called once per frame
	void Update () {

	}
}


// probabaly there's a better name
public class PatternManager {
    public void Grid(float start=0, float duration=10, int dots=40, float speed= 6, float width=50, float fadeinDuration=5, float fadeoutDuration=10, float particleSize=0.1f, ) {
        // load prefab? probably not?
        // there must be a grid script. let's work on that.
        //
        //during [start, start+duration]
        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time < endTime) {
            float progress = (Time.time - startTime) / duration;
            float angle = progress * speed;

            var coeff = new Dictionary<float, Complex>();
            coeff.put(1, Complex.FromDegrees(angle));
            coeff.put(-2, Complex.FromDegrees(-angle));
            coeff.put(3, Complex.FromDegrees(-angle));
            Complex[] samples = DFT.GenerateSamples(dots, coeff);
            Vector2[] positions = new Vector2[N];
            Vector2 center = ScreenUtil.center;
            for (int i = 0; i < N; i++) {
                positions[i] = center + new Vector2(samples[i].real, samples[i].img);
            }

            Render(positions, ShapeType.Circle, particleSize);

            yield return null;
        }
    }

    public void Swarm(float start=0, float duration=10, int dots=40, float speed= 6, float width=50, float fadeinDuration=5, float fadeoutDuration=10, float dotsize=0.1f) {
    }

    void Render(Vector2[] localPos, ShapeType shape, float particleSize) {
        // TODO create data, pass to renderer
        Vector2 center =
        for (int i=0; i<localPos.Length; i++) {
            localPos[i] +=
        }
        foreach (var local in localPos) {

        }
        Vector2 centerScreen;// TODO wth?
        if (shape == ShapeType.Circle) {
            CircleProperty2 prop = new CircleProperty2();
            ResourceLoader.InstantiateCircle();
        } else if (shape == ShapeType.Line) {
            LineProperty prop = new LineProperty();
            var obj = ResourceLoader.InstantiateLine(prop);
        } else if (shape == ShapeType.Rect) {
            ResourceLoader.InstantiateRect();
            var obj = ResourceLoader.InstantiateLine(prop);
        }
    }
}

public class Grid {

    public void set() {
    }
  //coeff2.put(3, Complex.FromDegrees(-baseSpeed*0.5));

  if (sec > s1 && sec < s2) {
      Complex[] samps = genSamples(coeff2, 200);
      drawEllipses(samps, 1000, 40, 0, height*3*p1-height/2);
  }
        // so scared, i don't want to work, i'd rather hide, it hurts, everything is a chance to fail
        // i acknowledge that. I really do. It sucks and it hurts to be exposed in such a way.
        // but shouldn't we bloom together?
}
