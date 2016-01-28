using System;

// use object initializer
public class MoonPattern: BasePattern {

    public float duration = 5f;
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 0.5f;
    public float sweepAngle = 25f;
    public float pauseBetweenBeams = 5f;
    public Type controllerType = typeof(MoonPatternController);

}
