using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
public static class ShapeZLevel {
    public const float Back = -5;
    public const float Default = 0;
    public const float Front = 5;
}
*/

public class Galuppi: MonoBehaviour {
    /* For certain duration, */

    public Color black;
    public Color gray;
    public Color yellow;
    public Color white;
    public Color boxColor;


    public int blackMeter;
    public int grayMeter;
    public int yellowMeter;

    public float blackWidth;
    public float grayWidth;
    public float yellowWidth;

    public float blackSpeed;
    public float graySpeed;
    public float yellowSpeed;

    public UnityEngine.UI.Text textBox;

    public TimeSignature timeSignature;

    public Camera camera;

    void Start() {
        StartCoroutine(RunBlack());
        StartCoroutine(RunBlack2());
        StartCoroutine(RunYellow2());
        StartCoroutine(RunGray());
        StartCoroutine(RunGray2());
        StartCoroutine(RunWhite());
        StartCoroutine(RunText());
        StartCoroutine(RunCamera());
//        StartCoroutine(RunBoxes());
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

    IEnumerator RunText() {
        var fontSize = new float[]{120, 180, 250, 270};
        foreach (var i in Times(5000)) {
            var offsetMax = textBox.rectTransform.offsetMax;
            offsetMax = new Vector2(offsetMax.x + Random.Range(-200f, 200f), offsetMax.y);
            textBox.rectTransform.offsetMax = offsetMax;

            var offsetMin = textBox.rectTransform.offsetMin;
            //offsetMin = new Vector2(offsetMin.x + Random.Range(-10f, 10f), offsetMin.y);
            textBox.rectTransform.offsetMin = offsetMin;
            textBox.fontSize = (int)RandomHelper.Pick(fontSize);
            yield return Rest(0, 3f);
        }
    }

    IEnumerator RunWhite() {
        foreach (var i in Times(5000)) {
            //var width = Random.Range(3ff);
            var width = 6;

            foreach (var _ in Times(1)) {
                _FixedLine(width:width, xOffset:Random.value, color:white, level:CurrentBeat);
                //yield return Rest(0, gap1);
            }
            yield return Rest(0, 4);
        }
    }

    IEnumerator RunBlack() {
        foreach (var i in Times(5000)) {
            //var speed = Random.Range(3f, 4f);
            var numberOfLines = Random.Range(1, 1);
            var gap1 = 1f;
            var gap2 = 2;

            var width = 1f;
            var speed = 20f;
            foreach (var _ in Times(numberOfLines)) {
                //_Line(Direction.Left, width, speed, gray, CurrentBeat + 64);
                //yield return Rest(0, 0.25f);
            }
            yield return Rest(0, 4);
        }
    }

    IEnumerator RunBlack2() {
        foreach (var i in Times(5000)) {
            //var speed = Random.Range(3f, 4f);
            var numberOfLines = Random.Range(1, 3);
            var gap1 = 1f;
            var gap2 = 2;

            foreach (var _ in Times(numberOfLines)) {
                var width = 26f;
                var speed = 18f;
                _Line(DirectionHelper.Random, width, speed, black, CurrentBeat + 16);
            }
            yield return Rest(0, 8);
        }
    }

    IEnumerator RunGray2() {
        foreach (var i in Times(5000)) {
            var width = Random.Range(0.3f, 2f);

            foreach (var _ in Times(3)) {
                _FixedLine(width:width, xOffset:Random.value, color:gray, level:CurrentBeat);
                //yield return Rest(0, gap1);
            }
            yield return Rest(0, 8);
        }
    }

    IEnumerator RunYellow2() {
        foreach (var i in Times(5000)) {
            var width = Random.Range(4f, 6f);

            foreach (var _ in Times(Random.Range(1, 2))) {
                _FixedLine(width:width, xOffset:Random.value, color:yellow, level:CurrentBeat-1);
                //yield return Rest(0, gap1);
            }
            yield return Rest(0, 4);
        }
    }

    IEnumerator RunGray() {
        yield return Rest(0, 1);
        foreach (var i in Times(500)) {
            foreach (var j in Times(4)) {
                _Line(Direction.Left, grayWidth, graySpeed, gray, CurrentBeat+1);
            }
            yield return Rest(0, 1);
        }
    }

    IEnumerator RunYellow() {
        foreach (var i in Times(5000)) {
            _Line(Direction.Right, yellowWidth, yellowSpeed, yellow, ShapeZLevel.Default);
            yield return Rest(0, yellowMeter);
        }
    }

    IEnumerator RunBoxes() {
        var angles = new float[] {0, 45, 90};
        foreach (var i in Times(5000)) {
            foreach (var _ in Times(Random.Range(3, 8))) {
                var scale = new Vector2(0.05f, 4f);
                scale = DirectionHelper.Random.Align(scale);
                _Box(scale,
                     new Vector2(CameraHelper.RandomXOffset(1, 0.25f),
                                 CameraHelper.RandomYOffset(1, 0.25f)),
                     boxColor,
                     RandomHelper.Pick(angles),
                     CurrentBeat + 8);
            }
            yield return Rest(0,8);
        }
    }

    void _Line(Direction dir, float width, float speed, Color color, float level) {
        var scale = new Vector2(width, CameraHelper.Height*2);
        var lp = new LineParams2 {
            position = CameraHelper.PerimeterPositionForMovingObject(dir, 0, scale, 0),
            color = color,
            scale = scale,
            rotation = 0,
            level = level};
        var mp = new MotionParams { velocity = dir.ToVelocity(speed) };

        NoteFactory.CreateLine(lp, mp);
    }

    void _FixedLine(float width, float xOffset, Color color, float level) {
        var scale = new Vector2(width, CameraHelper.Height*2);
        var lp = new LineParams2 {
            position = new Vector2(CameraHelper.Width*xOffset - CameraHelper.HalfWidth, 0),
            color = color,
            scale = scale,
            rotation = 0,
            level = level};
        var mp = new MotionParams();

        NoteFactory.CreateLine(lp, mp);
    }

    void _Box(Vector2 scale, Vector2 pos, Color color, float rotation, float level) {
        var lp = new LineParams2 {
            position = pos,
            color = color,
            scale = scale,
            rotation = rotation,
            level = level};
        var mp = new MotionParams();

        NoteFactory.CreateLine(lp, mp);
    }

    void _Line2(Direction dir, float width, float height, float offset, float speed, Color color, float level) {
        var scale = new Vector2(width, height);
        var lp = new LineParams2 {
            position = CameraHelper.PerimeterPositionForMovingObject(dir, 0, scale, 0),
            color = color,
            scale = scale,
            rotation = 0,
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

    int CurrentBeat {
        get {
            return (int)(Time.time / timeSignature.beatDuration) - 100;
        }
    }

    float BeatDurationInSeconds {
        get {
            return 60f / timeSignature.beatsPerMinute;
        }
    }
}


