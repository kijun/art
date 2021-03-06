using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AndataOld : MonoBehaviour {
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
        //StartCoroutine(RunTeal());
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
            AnimateRect(Direction.Down, 1, Vector2.one*0.5f, teal, CameraHelper.RandomXOffset(0.9f), level:0);
            yield return Rest(1);
        }

        int ornamentation = 1;
        float boundFactor = 0.1f;
        foreach (var i in Times(20)) {
            var progress = i / 20f;

            // Gradually add ornamentation
            var offset = CameraHelper.RandomXOffset(0.6f);
            var scale = Vector2.one * Random.Range(0.2f, 0.5f); // the big one
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
//   '       '    '        '    '    xx '    '    '
//   4    8    12   16   20   24   28   32   36   40   44   48   52 (end)
        var score = @"
0000 0001 0000 0000 0003 2001 0101 0002 3565 2000 0100 1000 0000 0000
1000 1000 0005 0004 0000 0050 0000 0102 3565 2000 1010 1000 0000 0000
0000 0010 0000 0002 2000 0101 0211 1102 3565 2333 0033 0101 0000 0000
0000 0000 0000 0000 2000 0100 0001 0002 3565 2000 0000 0000 0000 0000
";
        score = @"
5000 0001 0000 0000 0003 2001 0101 0002 3565 2000 0100 1000 0000 0000
0500 0001 0000 0000 0003 2001 0101 0002 3565 2000 0100 1000 0000 0000
0050 0010 0000 0002 2000 0101 0211 1102 3565 2333 0033 0101 0000 0000
0005 0000 0000 0000 2000 0100 0001 0002 3565 2000 0000 0000 0000 0000
";
        var ornamentation = @"
3020100010203040504
0101010101010101010
0102030405040302010
1111111111111111111
";
        score = score.Trim();
        var voicesInString = score.Split('\n').Select(str => str.Replace(" ", "")).ToArray();

        Debug.Log(voicesInString);
        int[,] voices = new int[voicesInString.Length, voicesInString[0].Length];
        int scoreLen = voices.GetLength(1);
        Debug.Log(voices.GetLength(0));
        Debug.Log(voices.GetLength(1));

        // initialize score Obj
        for (int i = 0; i < scoreLen; i++) {
            foreach (var j in Times(4)) {
                voices[j, i] = int.Parse(voicesInString[j][i].ToString());
            }
        }

        for (int i = 0; i<scoreLen; i++) {
            _Lines(Direction.Down, voices[0, i]);
            _Lines(Direction.Up, voices[1, i]);
            _Lines(Direction.Right, voices[2, i]);
            _Lines(Direction.Left, voices[3, i]);
            yield return Rest();
        }
    }

    IEnumerator RunHorizontal() {
        yield return Rest(6);
        _Lines(Direction.Right, 1);
        yield return Rest(3);
    }

    void _Lines(Direction dir, int count) {
        foreach (var i in Times((int)(count))) {
            float offset = 0;
            if (dir == Direction.Left|| dir == Direction.Right) {
                offset = CameraHelper.RandomYOffset(1f, 0.1f);
            } else {
                offset = CameraHelper.RandomXOffset(1f, 0.1f);
            }
            _Line(dir, defaultLineHeight, offset, -defaultLineSpeed*Random.value*0.1f);
        }
    }

    void _Line(Direction dir, float height, float offset, float deltaSpeed=0) {
        var scale = dir.Align(new Vector2(defaultLineWidth, height*(Random.value*0.7f+0.65f)));
        var lp = new LineParams2 {
            position = CameraHelper.PerimeterPositionForMovingObject(dir, offset, scale, 0),
            color = lineColor,
            scale = scale,
            rotation = 0,
            level = ShapeZLevel.Front};
        var mp = new MotionParams { velocity = dir.ToVelocity(defaultLineSpeed + deltaSpeed) };

        NoteFactory.CreateLine(lp, mp);
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

        NoteFactory.CreateLine(lp, mp);
    }

    void AnimateRect(Direction dir, float speed, Vector2 scale, Color color, Vector2 position, float rotation=0, float level=0) {
        var lp = new LineParams2 { position = position,
                                   color = color,
                                   scale = scale,
                                   rotation = rotation,
                                   level = level};
        var mp = new MotionParams { velocity = dir.ToVelocity(speed) };

        NoteFactory.CreateLine(lp, mp);
    }

    /** helpers, to be extracted into another class */

    IEnumerable<int> Times(int measures) {
        return Enumerable.Range(0, measures);
    }

    IEnumerator Rest(float measures=1, float beats = 0) {
        yield return new WaitForSeconds((measures * timeSignature.beatsPerMeasure + beats) * BeatDurationInSeconds);
    }

    float BeatDurationInSeconds {
        get {
            return 60f / timeSignature.beatsPerMinute;
        }
    }
}


