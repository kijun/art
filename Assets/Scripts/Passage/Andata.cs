using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ShapeZLevel {
    public const float Back = -5;
    public const float Default = 0;
    public const float Front = 5;
}

public class Andata : MonoBehaviour {
    /* For certain duration, */

    public Color orange;
    public Color red;
    public Color blue;
    public Color purple;
    public Color peach;

    public float defaultLineWidth;
    public float defaultLineSpeed;
    public float defaultLineHeight;

    public Color lineColor;

    public TimeSignature timeSignature;
    public Camera camera;

    void Start() {
        StartCoroutine(RunLineIntro());
//        StartCoroutine(RunShapes());
        StartCoroutine(RunShapes2());
        StartCoroutine(RunShapes3());
        StartCoroutine(RunShapes4());
        StartCoroutine(RunRisingGraph());
        StartCoroutine(RunCamera());
        StartCoroutine(RunCameraZoom());
        //StartCoroutine(RunCameraPosition());
    }
    IEnumerator RunCamera() {
        var choices = new float[]{0, 90, 180, 270};
        var restChoices = new float[]{2, 3, 4, 6, 8};
        yield return Rest(0, 4f);
        foreach (var i in Times(5000)) {
            camera.transform.rotation = Quaternion.Euler(0, 0, RandomHelper.Pick(choices));
            yield return Rest(0, RandomHelper.Pick(restChoices));
        }
    }

    IEnumerator RunCameraPosition() {
        var restChoices = new float[]{2.1f, 3.1f};
        var deltaPos = new float[]{5f, -5f};
        foreach (var i in Times(5000)) {
            var pos = camera.transform.position;
            var xd = RandomHelper.Pick(deltaPos);
            var yd = RandomHelper.Pick(deltaPos);

            if (Mathf.Abs(pos.x) > 15) {
                xd = -Mathf.Sign(pos.x) * Mathf.Abs(xd);
            }

            if (Mathf.Abs(pos.y) > 15) {
                yd = -Mathf.Sign(pos.y) * Mathf.Abs(yd);
            }


            camera.transform.position = camera.transform.position +
                new Vector3(xd, yd);

            yield return Rest(0, RandomHelper.Pick(restChoices));
        }
    }

    IEnumerator RunCameraZoom() {
        var cameraZoomLevels = new float [] {2, 5, 13};
        var restChoices = new float[]{3.4f, 5.4f};
        foreach (var i in Times(5000)) {
            camera.orthographicSize = RandomHelper.Pick(cameraZoomLevels);
            yield return Rest(0, RandomHelper.Pick(restChoices));
        }
    }

    IEnumerator RunShapes() {
        foreach (var i in Times(100)) {
            var randomColor = Color.Lerp(red, blue, Random.value); // closer to red
            var anim = NoteFactory.CreateRectInViewport(
                x:Random.value, y:Random.value, width:Random.value * 0.3f, height: Random.value * 0.3f, color: randomColor);
            Keyframe[] kff = KeyframeHelper.CreateKeyframes(
                0, 0,
                10, 1,
                20, 1,
                30, 0
            );

            anim.AddAnimationCurve(AnimationKeyPath.Opacity, new AnimationCurve(kff));
            anim.DestroyIn(31);
            yield return Rest(1, 0);
        }
    }

    IEnumerator RunShapes2() {
        Color[] colors = {orange, red, purple};
        // until measure 40
        foreach (var i in Times(1000)) {
            var randomColor = Color.Lerp(red, blue, Random.Range(0.5f, 1f));
            var anim = NoteFactory.CreateRectInViewport(
                x:Random.Range(0, 9)/10f+0.1f, y:Random.Range(0, 9)/10f + 0.1f, width:0.13f, height: 0.2f, color: randomColor, level: ShapeZLevel.Back+i/1000f);
            Keyframe[] kff = KeyframeHelper.CreateKeyframes(
                0, 0,
                Beat, 1,
                Beat*3, 1,
                Beat*4, 0
            );

            anim.DestroyIn(Beat*6);
            anim.AddAnimationCurve(AnimationKeyPath.Opacity, new AnimationCurve(kff));
            yield return Rest(0, 1f);
        }
    }

    IEnumerator RunShapes3() {
        yield return Rest(8);
        foreach (var i in Times(1000)) {
            foreach (var _ in Times(1)) {
                var randomColor = Color.Lerp(orange, red, Random.value); // closer to red
                var anim = NoteFactory.CreateRectInViewport(
                    x:0.5f, y:Random.Range(0, 10)/10f, width:1f, height: 0.03f, color: randomColor);
                var maxOpacity = Random.Range(0.7f, 1f);
                Keyframe[] kff = KeyframeHelper.CreateKeyframes(
                    0, 0,
                    Beat*2, maxOpacity,
                    Beat*6, maxOpacity,
                    Beat*8, 0
                );

                anim.DestroyIn(Beat*10);
                anim.AddAnimationCurve(AnimationKeyPath.Opacity, new AnimationCurve(kff));
            }
            yield return Rest(0, 2f);
        }
    }

    IEnumerator RunShapes4() {
        yield return Rest(24);
        foreach (var i in Times(1000)) {
            foreach (var _ in Times(1)) {
                var randomColor = Color.Lerp(orange, red, Random.value); // closer to red
                var anim = NoteFactory.CreateRectInViewport(
                    x:0.5f, y:Random.Range(0, 10)/10f, width:0.7f, height: 0.1f, color: randomColor, rotation: Random.value*360);
                var maxOpacity = Random.Range(0.5f, 1f);
                Keyframe[] kff = KeyframeHelper.CreateKeyframes(
                    0, 0,
                    Beat, maxOpacity,
                    Beat*2, maxOpacity,
                    Beat*3, 0
                );

                anim.DestroyIn(Beat*4);
                anim.AddAnimationCurve(AnimationKeyPath.Opacity, new AnimationCurve(kff));
            }
            yield return Rest(0, 4f);
        }
    }

    IEnumerator RunRisingGraph() {
        yield return Rest(24);
        foreach (var i in Times(1000)) {
            CreateLineGraph();
            yield return Rest(4);
        }
    }


    IEnumerator RunOrange() {
        foreach (var i in Times(10)) {
            AnimateRect(Direction.Down, 1, Vector2.one*0.5f, orange, CameraHelper.RandomXOffset(0.9f), level:0);
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
                        new Color[]{orange, blue, red}[Random.Range(0, Mathf.CeilToInt(3*progress))],
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
1010 0010 1001 1000 0001 0001 0101 0001 0100 2000 0100 1000 0000 0100
0000 1000 0000 0010 1100 0010 0000 1000 1000 0010 0010 0010 1000 0000
0000 0000 0000 0000 0000 0000 0000 0000 0010 0000 0000 0000 0010 0000
0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000
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

    float Beat {
        get {
            return 60f / timeSignature.beatsPerMinute;
        }
    }

    void CreateLineGraph () {

        int numberOfLines = 10;

        float baseLength = 2;
        float lengthIncrement = 0.5f;
        int lengthInflectionCount = 5;

        float baseSpeed = 0.5f;
        float speedIncrement = 0.04f;
        int speedInflectionCount = 2;

        float baseWidth = 0.05f;
        float baseGap = 0.1f;


        var equalizerWidth = numberOfLines * baseWidth + (numberOfLines - 1) * baseGap;
        var lengthInflections = RandomHelper.Points(0, numberOfLines, lengthInflectionCount);
        lengthInflections.Add(int.MaxValue);
        var nextLenInflection = lengthInflections[0];
        lengthInflections.RemoveAt(0);

        var speedInflections = RandomHelper.Points(0, numberOfLines, speedInflectionCount);
        speedInflections.Add(int.MaxValue);
        var nextSpeedInflection = speedInflections[0];
        speedInflections.RemoveAt(0);

        var currXPos = (CameraHelper.Width - equalizerWidth) * Random.value - CameraHelper.HalfWidth;
        var currLenDir= 1;
        var currLen = baseLength;
        var currSpeedDir = 1;
        var currSpeed = baseSpeed;

        for (int i = 0; i < numberOfLines; i++) {
            var line = new LineParams2();
            line.length = currLen;
            line.width = baseWidth;
            line.x = currXPos;
            line.y = (CameraHelper.Height+baseLength*2) / -2f - 2f;
            line.color = lineColor;
            NoteFactory.CreateLine(line, new MotionParams{velocity= Vector2.up * currSpeed});

            // next
            currXPos += baseWidth + baseGap;
            currLen += currLenDir * lengthIncrement;
            currSpeed += speedIncrement * currSpeedDir;

            if (nextLenInflection <= i) {
                // reverse dir
                currLenDir *= -1;
                nextLenInflection =lengthInflections[0];
                lengthInflections.RemoveAt(0);
            }

            if (nextSpeedInflection <= i) {
                // reverse dir
                currSpeedDir *= -1;
                nextSpeedInflection = speedInflections[0];
                speedInflections.RemoveAt(0);
            }
        }
    }

    T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0,A.Length));
        return V;
    }
}


