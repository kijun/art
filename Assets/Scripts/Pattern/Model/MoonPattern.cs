using System;

// use object initializer
public class MoonPattern: BasePattern {

    public float duration = 10f;
    public float sweepDuration = 5f;
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 0.5f;
    public float sweepAngle = 25f;
    public float pauseBetweenBeams = 5f;
    public override Type controllerType {
        get {
            return typeof(MoonPatternController);
        }
    }
}
