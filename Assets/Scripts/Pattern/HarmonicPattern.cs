using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// does a harmonic pattern include shapes?
public class HarmonicSequenceGenerator {
    public float duration;
    public int N;
    public HarmonicFrequency[] frequencies;

    public HarmonicSequenceGenerator(float duration, int N, HarmonicFrequency[] frequencies) {
        this.duration = duration;
        this.N = N;
        this.frequencies = frequencies;
    }

    public static HarmonicSequenceGenerator Star(float duration, int N) {
        HarmonicFrequency[] freqs = {HarmonicFrequency.Constant(1, new Complex(0, 1))};
        return new HarmonicSequenceGenerator(duration, N, freqs);
    }

    public Complex[] GenerateSamples(float elapsedTime, float progress) {
        var coeff = new Dictionary<float, Complex>();
        foreach (var fp in frequencies) {
            float k = fp.frequencyTransform(progress, elapsedTime);
            Complex Xk = fp.coefficientTransform(progress, elapsedTime);
            coeff.Add(k, Xk);
        }
        return DFT.GenerateSamples(coeff, N);
    }
}

public class HarmonicSequenceVisualizer {
    // TODO fleet needs to move, but we'll figure out how later
    public CreateShapeDelegate createShape;

    public HarmonicSequenceVisualizer (CreateShapeDelegate createShape) {
        this.createShape = createShape;
    }

    public static HarmonicSequenceVisualizer BaseCircle(CircleProperty baseCircleProperty, float scale) {
       CreateShapeDelegate createShape = delegate(Complex sample, Vector2 center) {
            return new CircleProperty(
                    center: center+scale*new Vector2(sample.real, sample.img),
                    diameter: baseCircleProperty.diameter,
                    color: baseCircleProperty.color,
                    border: baseCircleProperty.border);
        };

        return new HarmonicSequenceVisualizer(createShape);
    }

    public ShapeProperty[] SamplesToShapes(Complex[] samples, Vector2 center) {
        var shapes = new ShapeProperty[samples.Length];
        for (int i = 0; i<samples.Length; i++) {
            shapes[i] = createShape(samples[i], center);
        }
        return shapes;
    }
}


// could make some default delegates
public delegate float FrequencyTransformDelegate(float progress, float elapsedTime);
public delegate Complex CoefficientTransformDelegate(float progress, float elapsedTime);

public class HarmonicFrequency {
    public FrequencyTransformDelegate frequencyTransform;
    public CoefficientTransformDelegate coefficientTransform;

    public HarmonicFrequency(FrequencyTransformDelegate freq, CoefficientTransformDelegate coeff) {
        frequencyTransform = freq;
        coefficientTransform = coeff;
    }

    public static HarmonicFrequency Constant(float frequency, Complex coeff) {
        FrequencyTransformDelegate freqDel = delegate(float progress, float elapsedTime) { return frequency; };
        CoefficientTransformDelegate coeffDel = delegate(float progress, float elapsedTime) { return coeff; };
        return new HarmonicFrequency(freqDel, coeffDel);
    }
}



// how about if you want to draw a line between dots? yeah that's a real concern right?
public delegate ShapeProperty CreateShapeDelegate(Complex sample, Vector2 center);

/*
public struct DFTSampleParams {
    public float duration;
    public int N;
    public DFTFrequencyParams[] frequencies;
    public DFTSampleParams(float duration=100f, int N=50) {
    }
}
*/

/*
public struct DFTRenderParams {
    public Direction fleetCenter;
    public Vector2 fleetCenterInitialDisplacement;
    public Interpolator fleetCenterInterpolator;
   // = new ConstantInterpolator();
    public DFTCreateShapeDelegate createShape;
}

*/
