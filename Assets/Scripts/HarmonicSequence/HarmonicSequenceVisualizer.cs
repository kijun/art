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

    public static HarmonicSequenceVisualizer BaseLine(LineProperty shapeProperty, Vector2 direction, float scale) {
       SampleToShapeDelegate createShape = delegate(Complex sample, Vector2 center) {
            return new LineProperty(
                    //center: center+new Vector2(scale*sample.real, scale*sample.img),//scale*sample.img),
                    center: center+new Vector2(scale*sample.real, scale*sample.img),//scale*sample.img),
                    length: scale*sample.img,
                    width: shapeProperty.width*sample.real,
                    //angle: new Vector2(sample.magnitude, sample.img).AngleInDegrees(),
                    color: shapeProperty.color,
                    border: shapeProperty.border);
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

