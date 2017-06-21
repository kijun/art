using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public struct LineParams {
    public float x;
    public float y;
    public float length;
    public float width;
    public Color color;
    public Vector2 velocity;

    public Vector2 position {
        get { return new Vector2(x, y); }
    }
    public Vector2 scale {
        get { return new Vector2(width, length); }
    }
}

public class CellEqualizer : BaseCell {
    public int numberOfLines;

    public float baseLength;
    public float lengthIncrement;
    public int lengthInflectionCount;

    public float baseSpeed;
    public float speedIncrement;
    public int speedInflectionCount;

    public float baseWidth;
    public float baseGap;

    /** RENDERING **/
    public Color color;

    public bool test;

    void Start() {
        if (test) {
            Run();
        }
    }

    public override void Run() {
        StartCoroutine(_Run());
    }

    public IEnumerator _Run() {
        // Generate
        var lineParams = GenerateLineParams();

        foreach (var line in lineParams) {
            FireLine(line);
        }

    //    foreach (line

        /*

        var x = (inGameWidth+ scales[0].x) / 2f * Random.Range(-0.7f, 0.7f);
        var seq1 = Random.Range(1, 4);
        for (int i = 0; i < seq1; i++) {
            FireLine(x, i);
            x += scales[0].x * 2f;
        }
        */
        yield return null;
    }

    List<LineParams> GenerateLineParams() {

        var lines = new List<LineParams>();

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
            var line = new LineParams();
            line.length = currLen;
            line.width = baseWidth;
            line.x = currXPos;
            line.y = (CameraHelper.Height+baseLength*2) / -2f;
            line.velocity = Vector2.up * currSpeed; // for now, same speed

            lines.Add(line);

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

        return lines;
    }

    void FireLine(LineParams lineP) {
        var p = CreatePlane(Quaternion.identity);
        p.localScale = lineP.scale;
        p.level = level;
        p.velocity = lineP.velocity;
        p.position = lineP.position;
        p.color = color;
    }

    Animatable2 CreatePlane(Quaternion entryAngle) {
        return Instantiate<Animatable2>(prefab, Vector2.one, entryAngle);
    }

    T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0,A.Length));
        return V;
    }

}




