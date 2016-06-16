using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO stage
public class Main : MonoBehaviour {

    List<IEnumerator> patterns = new List<IEnumerator>();

	// Use this for initialization
	void Start () {
        SetupLevel();
        StartLevel();
	}

    void SetupLevel() {
        Add(BasePattern.Grid(duration: 20));
        Add(BasePattern.Grid(duration: 20, size:3, speed:2, dots:100, particleSize:0.02f));
        //PatternManager.Swarm(duration: 20);
    }

    void Add(IEnumerator basePattern) {
        patterns.Add(basePattern);
    }

    void StartLevel() {
        foreach (var bp in patterns) {
            StartCoroutine(bp);
        }
    }

	// Update is called once per frame
	void Update () {

	}
}

// could make some default delegates
delegate float FrequencyTransformDelegate(float progress, float elapsedTime);
delegate Complex CoefficientTransformDelegate(float progress, float elapsedTime);

/* TODO
- transform with time?
    - k
    - amplitude
    - phase
    - function
        - back and forth
        - accelerated
        - linear
*/
public struct DFTFrequencyParams {
    public FrequencyTransformDelegate frequencyTransform;
    public CoefficientTransformDelegate coefficientTransform;
}


// there must be scale embedded with each parameter i think
public enum FFF {
    None, XPos, YPos, Width, Height, Angle
}

// how about if you want to draw a line between dots? yeah that's a real concern right?
public struct DFTRenderParams {
    public ShapeType shape;
    public float size;
    public FFF realTransform;
    public FFF imgTransform;
    public FFF sampleIndexTransform;
    public Direction startDirection;
    public Vector2 displacement;
    // this might not be a simple mapping
}

public struct DFTSampleParams {}
public struct DFTRenderParams {}

public struct DFTParams {
    public float duration;
    public int N;
    public DFTFrequencyParams[] frequencies;
    public DFTRenderParams renderParams;
    // TODO size interpolator
    // TODO color interpolator
}


// TODO better name
public class BasePattern {

    public static IEnumerator RunDFT(DFTParams param) {
        float startTime = Time.time;
        float endTime = startTime + param.duration;

        while (Time.time < endTime) {
            /* create samples */
            float elapsedTime = Time.time - startTime;
            float progress = elapsedTime / param.duration;

            var coeff = new Dictionary<float, Complex>();
            foreach (var fp in param.frequencies) {
                float k = fp.frequencyTransform(progress, elapsedTime);
                Complex Xk = fp.coefficientTransform(progress, elapsedTime);
                coeff.Add(k, Xk);
            }
            Complex[] samples = DFT.GenerateSamples(coeff, param.N);

            /* render */
            Vector2 center = ScreenUtil.ScreenLocationToWorldPosition(
                    param.renderParams.startDirection,
                    param.renderParams.displacement);

            // samples => shape properties => game objects

            // TODO calculate position per sample - however this could be something
            // totally different, it could return a cirle, rect, etcetc then what?
            //TODO Render
        }

        // render
    }

    public static DrawSamples(DFTParams param) {
    }

    public static ShapeParams SamplesToShapes(Complex[] samples) {
        // TODO how?
        // what are the params?
        //
        // shape type and then yeah.
        // smart renderer, creates shapes to draw
    }


    // TODO too many args? color and particle etc should be taken out as a separate parameter.
    // TODO maybe like a chain something returns where things should be drawn,
    // and the renderer works on it, perhaps
    // points -> rendering
    // TODO remove static
    public static IEnumerator Grid(
            float start=0,
            float duration=10,
            int dots=10,
            float speed=6,
            float size=7,
            float fadeinDuration=5,
            float fadeoutDuration=10,
            float particleSize=0.1f,
            Direction startDirection=Direction.Center,
            Vector2 displacement = new Vector2(),
            LinearInterpolator interpolatePosition = null)
    {

        // TODO fix
        float startTime = Time.time + start;
        float endTime = startTime + duration;

        Object[] renderedObjects = null;

        while (Time.time < endTime) {
            float elapsedTime = Time.time - startTime;
            float progress = elapsedTime / duration;
            // TODO what about rotation interpolation
            float angle = progress * speed * 2 * Mathf.PI;

            var coeff = new Dictionary<float, Complex>();
            coeff.Add(1, Complex.FromRadian(angle));
            coeff.Add(-2, Complex.FromRadian(-angle));
            coeff.Add(3, Complex.FromRadian(-angle));
            Complex[] samples = DFT.GenerateSamples(coeff, dots);
            Vector2[] positions = new Vector2[dots];
            Vector2 center = ScreenUtil.ScreenLocationToWorldPosition(startDirection,displacement);
            // TODO maybe apply interpolator for each point? (nonlinear transform)
            if (interpolatePosition != null) {
                center = interpolatePosition.Interpolate(center, progress, elapsedTime);
            }
            for (int i = 0; i < dots; i++) {
                positions[i] = center + size*new Vector2(samples[i].real, samples[i].img);
            }

            renderedObjects = Render(positions, ShapeType.Circle, particleSize, renderedObjects);

            yield return null;
        }
    }

    public void Swarm(float start=0, float duration=10, int dots=40, float speed= 6, float width=50, float fadeinDuration=5, float fadeoutDuration=10, float dotsize=0.1f) {
    }

    // TODO remove static
    public static Object[] Render(
            Vector2[] pos,
            ShapeType shape,
            float particleSize,
            Object[] rendered) // TODO refactor like hell
    {
        // TODO got to figure out object recycling
        // TODO smooth interpolator?
        if (rendered == null) {
            rendered = new Object[pos.Length];
        }
        if (rendered.Length != pos.Length) {
            Debug.LogError("wrong number of objects dude");
            return null;
        }


        if (shape == ShapeType.Circle) {
            for (int i=0; i<pos.Length; i++) {
                var p = pos[i];
                CircleProperty2 prop = new CircleProperty2(center:p);
                if (rendered[i] == null) {
                    rendered[i] = ResourceLoader.InstantiateCircle(prop);
                } else {
                    // TODO rewrite desperately needed
                    ((CircleProperty)rendered[i]).center = prop.center;
                }
                // probably should need to call this;
                // TODO ineffective so ineffective
                ((CircleProperty)rendered[i]).OnUpdate();
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
        return rendered;
    }
}
