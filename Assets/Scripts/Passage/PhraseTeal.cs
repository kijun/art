using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ShapeZLevel {
    public const float Back = -5;
    public const float Default = 0;
    public const float Front = 5;
}

public class PhraseTeal : MonoBehaviour {
    /* For certain duration, */

    public Color teal;
    public Color red;
    public Color blue;

    public float defaultLineWidth;
    public float defaultLineSpeed;
    public float defaultLineHeight;

    public Color lineColor;

    public TimeSignature timeSignature;

    void Start() {
//        StartCoroutine(RunTeal());
        StartCoroutine(RunLineIntro());
//        StartCoroutine(RunBlue());
 //       StartCoroutine(RunRed());
        /*
        StartCoroutine(RunBlue());
        StartCoroutine(RunLine1());
        StartCoroutine(RunLine2());
        */
    }

    IEnumerator RunTeal() {
        foreach (var i in Times(10)) {
            AnimateRect(Direction.Down, 1, Vector2.one*2, teal, CameraHelper.RandomXOffset(0.9f), level:0);
            yield return Rest(1);
        }

        int ornamentation = 1;
        float boundFactor = 0.1f;
        foreach (var i in Times(20)) {
            var progress = i / 20f;

            // Gradually add ornamentation
            var offset = CameraHelper.RandomXOffset(0.6f);
            var scale = Vector2.one * Random.Range(2, 3); // the big one
            var speed = 1;

            var boundingRect = CameraHelper.BoundingRectOnPerimeter(
                    Location.Top, CameraHelper.Width * boundFactor, CameraHelper.Height * boundFactor, offset);
            var foundationRect = RectHelper.RectFromCenterAndSize(boundingRect.center, scale);
            var formation = GenerateFormation(foundationRect, boundingRect, Random.Range(1, 3));
            foreach (var rect in formation) {
                var lineSpeed =  speed * Random.Range(1f-0.3f*progress, 1f + 0.3f*progress);
                var level = 0;
                if (rect == formation[0]) {
                    lineSpeed = speed;
                    level = -1;
                }
                AnimateRect(
                        Direction.Down,
                        lineSpeed,
                        rect.size,
                        new Color[]{teal, blue, red}[Random.Range(0, Mathf.CeilToInt(3*progress))],
                        rect.center,
                        0);
            }
            yield return Rest(2);

            ornamentation++;
            boundFactor += 0.05f;
        }
    }

    IEnumerator RunLineIntro() {
        // 16 measures for the first minute
        // 4, 5, 2
        _Lines(Direction.Up, 1);
        yield return Rest(4);
        _Lines(Direction.Up, 1);
        yield return Rest(2);
        _Lines(Direction.Right, 1);
        yield return Rest(3);
        _Lines(Direction.Left, 1);
        yield return Rest(2);
        _Lines(Direction.Up, 4);
        yield return Rest(2);
        _Lines(Direction.Up, 2);
        yield return Rest(1);
        _Lines(Direction.Right, 1);
        yield return Rest(1);
        _Lines(Direction.Down, 1);
        yield return Rest(1);
        _Lines(Direction.Down, 1);
        yield return Rest(1);

        _Lines(Direction.Up, 4);
        yield return Rest(2);

        _Lines(Direction.Up, 4);
    }

    void _Lines(Direction dir, int count) {
        foreach (var i in Times(count)) {
            float offset = 0;
            if (dir == Direction.Left|| dir == Direction.Right) {
                offset = CameraHelper.RandomYOffset(0.8f);
            } else {
                offset = CameraHelper.RandomXOffset(0.8f);
            }
            _Line(dir, defaultLineHeight, offset);
        }
    }

    void _Line(Direction dir, float height, float offset, float deltaSpeed=0) {
        var scale = dir.Align(new Vector2(defaultLineWidth, height));
        var lp = new LineParams2 {
            position = CameraHelper.PerimeterPositionForMovingObject(dir, offset, scale, 0),
            color = lineColor,
            scale = scale,
            rotation = 0,
            level = ShapeZLevel.Front};
        var mp = new MotionParams { velocity = dir.ToVelocity(defaultLineSpeed + deltaSpeed) };

        MotionController.Animate(lp, mp);
    }



    IEnumerator RunBlue() {
        foreach (var i in Times(10)) {
            AnimateRect(Direction.Right, 3, Vector2.one, blue, CameraHelper.RandomYOffset(0.6f));
            yield return Rest(0, timeSignature.beatsPerMeasure/2f);
        }

        foreach (var i in Times(20)) {
            yield return Rest(1);
        }
    }

    IEnumerator RunRed() {
        foreach (var i in Times(10)) {
            AnimateRect(Direction.Up, 3, Vector2.one, red, CameraHelper.RandomXOffset(0.6f));
            yield return Rest(0, timeSignature.beatsPerMeasure);
        }

        foreach (var i in Times(20)) {
            yield return Rest(1);
        }
    }

    List<Rect> GenerateFormation(Rect foundation, Rect boundary, int count, bool nonOverlapping=true) {
        var rects = new List<Rect>();
        rects.Add(foundation);
        int retries = 0;
        while (rects.Count <= count && retries < 1000) {
            var enclosed = new Vector2(
                    foundation.width.RandomFraction(0.05f, 0.5f),
                    foundation.height.RandomFraction(0.05f, 0.8f));
            var candidate = RectHelper.RectFromCenterAndSize(boundary.RandomPosition(), enclosed);
            if (nonOverlapping && rects.Any(r => r.Overlaps(candidate))) {
                retries++;
                continue;
            }
            rects.Add(candidate);
        }
        return rects;
    }

    void AnimateRect(Direction dir, float speed, Vector2 scale, Color color, float offset=0, float rotation=0, float level=0) {
        var lp = new LineParams2 { position = CameraHelper.PerimeterPositionForMovingObject(dir, offset, scale, rotation),
                                   color = color,
                                   scale = scale,
                                   rotation = rotation,
                                   level = level};
        var mp = new MotionParams { velocity = dir.ToVelocity(speed) };

        MotionController.Animate(lp, mp);
    }

    void AnimateRect(Direction dir, float speed, Vector2 scale, Color color, Vector2 position, float rotation=0, float level=0) {
        var lp = new LineParams2 { position = position,
                                   color = color,
                                   scale = scale,
                                   rotation = rotation,
                                   level = level};
        var mp = new MotionParams { velocity = dir.ToVelocity(speed) };

        MotionController.Animate(lp, mp);
    }

    /** helpers, to be extracted into another class */

    IEnumerable<int> Times(int measures) {
        return Enumerable.Range(0, measures);
    }

    IEnumerator Rest(float measures, float beats = 0) {
        yield return new WaitForSeconds((measures * timeSignature.beatsPerMeasure + beats) * BeatDurationInSeconds);
    }

    float BeatDurationInSeconds {
        get {
            return 60f / timeSignature.beatsPerMinute;
        }
    }
}


