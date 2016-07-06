using UnityEngine;

// TODO proper
delegate ShapeProperty SampleToShapeDelegate(Complex sample, Vector2 center);

public class HarmonicSequenceVisualizer {
    // TODO fleet needs to move, but we'll figure out how later
    SampleToShapeDelegate createShape;

    HarmonicSequenceVisualizer (SampleToShapeDelegate createShape) {
        this.createShape = createShape;
    }

    public static HarmonicSequenceVisualizer BaseCircle(CircleProperty baseCircleProperty, float scale) {
       SampleToShapeDelegate createShape = delegate(Complex sample, Vector2 center) {
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

