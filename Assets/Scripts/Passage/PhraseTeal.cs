using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhraseTeal : MonoBehaviour {
    /* For certain duration, */

    public Color teal;
    public Color red;
    public Color blue;

    public TimeSignature timeSignature;

    void Start() {
        StartCoroutine(RunTeal());
//        StartCoroutine(RunBlue());
 //       StartCoroutine(RunRed());
        /*
        StartCoroutine(RunBlue());
        StartCoroutine(RunLine1());
        StartCoroutine(RunLine2());
        */
    }

    IEnumerator RunTeal() {
        foreach (var i in Times(0)) {
            AnimateRect(Direction.Down, 5, Vector2.one, teal, CameraHelper.RandomXOffset(0.6f), level:0);
            yield return Rest(1);
        }

        foreach (var i in Times(20)) {
            // Gradually add ornamentation
            var offset = CameraHelper.RandomXOffset(0.6f);
            var scale = Vector2.one * Random.Range(2, 3); // the big one
            var speed = 2;

            var boundingRect = CameraHelper.BoundingRectOnPerimeter(
                    Location.Top, CameraHelper.Width * 0.5f, CameraHelper.Height * 0.5f, offset);
            var foundationRect = RectHelper.RectFromCenterAndSize(boundingRect.center, scale);
            var formation = GenerateFormation(foundationRect, boundingRect, Random.Range(30, 40));
            foreach (var rect in formation) {
                var lineSpeed =  speed * Random.Range(0.9f, 1.3f);
                var level = 0;
                if (rect == formation[0]) {
                    lineSpeed = speed;
                    level = -1;
                }
                Debug.Log(level);
                AnimateRect(
                        Direction.Down,
                        lineSpeed,
                        rect.size,
                        new Color[]{teal, blue, red}[Random.Range(0, 3)],
                        rect.center,
                        Random.Range(0, 270), level);
            }
            yield return Rest(1);
        }
    }

    List<Rect> GenerateFormation(Rect foundation, Rect boundary, int count, bool nonOverlapping=true) {
        var rects = new List<Rect>();
        rects.Add(foundation);
        Debug.Log("Foundation " + foundation);
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
            //Debug.Log("Added " + candidate);
        }
        Debug.Log(retries);
        return rects;
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
        Debug.Log(level);
        Debug.Log(lp.level);
        var mp = new MotionParams { velocity = dir.ToVelocity(speed) };

        MotionController.Animate(lp, mp);
    }

    /*
    IEnumerator RunRed() {
        // -> up
    }

    IEnumerator RunYellow() {
        // -> down
    }

    IEnumerator RunLine1() {
        // horizontal
        StartCoroutine(CellA());
        yield return Rest(2);
    }

    IEnumerator RunLine2() {
        // vertical
    }

    //void Ornamentation(Line
    // ornamentation
    //
    //
    //
    */

    IEnumerable<int> Times(int measures) {
        return Enumerable.Range(0, measures);
    }

    IEnumerator Rest(int measures, float beats = 0) {
        yield return new WaitForSeconds((measures * timeSignature.beatsPerMeasure + beats) * BeatDurationInSeconds);
    }

    float BeatDurationInSeconds {
        get {
            return 60f / timeSignature.beatsPerMinute;
        }
    }
}


