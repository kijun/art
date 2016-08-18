using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO stage
//
[System.Serializable]
public struct TempRotParams {
    public float k;
    public float secPerRotation;
    public float scale;
}

public class Main : MonoBehaviour {

    public int N = 20;
    public TempRotParams[] rotParams;
    public CircleProperty circleProp = new CircleProperty(
                            diameter: 7,
                            color:new Color(235, 205, 205, 0.3f)
                        );
    public LineProperty lineProp = new LineProperty(
                            color:new Color(235, 205, 205, 0.3f)
                        );

	// Use this for initialization
	void Start () {
        SetupLevel();
	}

    void SetupLevel() {
        StartCoroutine(Test());
    }

    IEnumerator Test() {
        float duration = 100000;

        float startTime = Time.time;
        float endTime = startTime + duration;
        ShapeRenderer[] renderedObjects = null;
        ShapeRenderer[] renderedObjects2 = null;

        while (Time.time < endTime) {
            // linq?
            var freqs = new HarmonicFrequencyGenerator[rotParams.Length];
            var viz = HarmonicSequenceVisualizer.BaseCircle(circleProp, 1);
            var viz2 = HarmonicSequenceVisualizer.BaseLine(lineProp, new Vector2(0, 1), 1);
            //
//            SampleToShapeDelegaet
//                alksdjfalkdsf

            for (int i = 0; i<rotParams.Length; i++) {
                var param = rotParams[i];
                freqs[i] = HarmonicFrequencyGenerator.Rotation(
                        frequency:param.k,
                        secPerRotation:param.secPerRotation,
                        scale: param.scale);
            }
            var seq = new HarmonicSequenceGenerator(duration, N, freqs);

            float elapsedTime = Time.time - startTime;
            float progress = elapsedTime / seq.duration;
            // TODO what about rotation interpolation
            Complex[] samples = seq.GenerateSamples(progress:progress, elapsedTime:elapsedTime);

            Vector2 center = ScreenUtil.ScreenLocationToWorldPosition(Direction.Center, Vector2.zero);

            renderedObjects = RenderShapes(viz.SamplesToShapes(samples, center), renderedObjects);
            renderedObjects2 = RenderShapes(viz2.SamplesToShapes(samples, center), renderedObjects2);

            yield return null;
        }
    }

    ShapeRenderer[] RenderShapes(ShapeProperty[] shapes, ShapeRenderer[] rendered) {
        if (rendered == null) {
            rendered = new ShapeRenderer[shapes.Length];
        }
        // TODO extend at will
        if (rendered.Length != shapes.Length) {
            Debug.Log("new cache");
            foreach (var o in rendered) {
                Destroy(o.gameObject);
            }
            rendered = new ShapeRenderer[shapes.Length];
        }

        for (int i=0; i<shapes.Length; i++) {
            if (rendered[i] == null) {
                rendered[i] = ShapeGOFactory.InstantiateShape(shapes[i]);
            } else {
                ShapeGOFactory.UpdateShapeProperty((ShapeRenderer)rendered[i], shapes[i]);
            }
        }
        return rendered;
    }

	// Update is called once per frame
	void Update () {

	}
}

// TODO better name
public class BasePattern {

//    public static IEnumerator RunDFT(DFTSampleParams sampleParams, DFTRenderParams renderParams) {
//        float startTime = Time.time;
//        float endTime = startTime + sampleParams.duration;
//
//        Object[] renderedShapes = null;
//
//        while (Time.time < endTime) {
//
//            float elapsedTime = Time.time - startTime;
//            float progress = elapsedTime / sampleParams.duration;
//
//            /* create samples */
//            Complex[] samples = GenerateSamples(sampleParams, elapsedTime, progress);
//
//            /* convert to shapes */
//            ShapeProperty[] shapeProps = SamplesToShapes(samples, renderParams, elapsedTime, progress);
//
//            /* render shapes */
//            renderedShapes = RenderShapes(shapeProps, renderedShapes);
//
//            yield return null;
//        }
//    }


    // not sure what parameters it should use
    // TODO cache properties
    /*
    public static ShapeProperty[] SamplesToShapes(
            Complex[] samples,
            DFTRenderParams renderParams,
            float elapsedTime,
            float progress) {
        // TODO calculate position per sample
        // TODO doesn't work when camera moves
        Vector2 center = ScreenUtil.ScreenLocationToWorldPosition(
                renderParams.fleetCenter,
                renderParams.fleetCenterInitialDisplacement);
        center = renderParams.fleetCenterInterpolator.Interpolate(center, progress, elapsedTime);

        // TODO and also i used to have a interpolator - where does it go?

        var shapes = new ShapeProperty[samples.Length];

        for (int i = 0; i<samples.Length; i++) {
            shapes[i] = renderParams.createShape(samples[i], center);
        }

        return shapes;
    }

    public static Object[] RenderShapes(ShapeProperty[] shapeProps, Object[] renderedShapes) {
        /*
        // CircleRenderer - lien renderer
        if (renderedShapes == null) {
            renderedShapes = new Object[shapeProps.Length];
        }

        foreach (var prop in shapeProps) {
            var shapeType = prop.GetType();
            if (shapeType == typeof(CircleProperty)) {

            } else if (shapeType == typeof(LineProperty)) {
            } else if (shapeType == typeof(RectProperty)) {
            }
        }
        return renderedShapes;
    }
        */

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
                var prop = new CircleProperty(
                        center:p,
                        diameter:particleSize,
                        color:new Color(235, 205, 205, 0.3f),
                        border: new BorderProperty(
                            style:BorderStyle.Solid,
                            //color: new Color(255, 255, 255,0.3f),
                            thickness: 5f));
                if (rendered[i] == null) {
                    var circle = ResourceLoader.InstantiateCircle(prop);
                    circle.RenderAndUpdatePropertyIfNeeded();
                    rendered[i] = circle;
                } else {
                    var circle = (CircleRenderer)rendered[i];
                    circle.property = prop;
                    circle.RenderAndUpdatePropertyIfNeeded();
                    // TODO rewrite desperately needed
                    //((CircleProperty)rendered[i]).center = prop.center;
                }
                // probably should need to call this;
                // TODO ineffective so ineffective
                //((CircleProperty)rendered[i]).OnUpdate();
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
