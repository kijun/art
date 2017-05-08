using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CellRangeChart : BaseCell {
    public int numberOfLines;

    public float baseLength;
    public float lengthIncrement;
    public int lengthInflectionCount;

    public Vector2 baseVelocity;
    public Vector2 velocityIncrement;
    public int velocityInflectionCount;

    public float baseWidth;
    public float baseGap;

    public int numberOfLargeGaps;
    public float largeGap;

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

        yield return null;
    }

    List<LineParams> GenerateLineParams() {

        var lines = new List<LineParams>();

        var equalizerWidth = numberOfLines * baseWidth + (numberOfLines - 1) * baseGap;
        var lengthInflections = RandomHelper.Points(0, numberOfLines, lengthInflectionCount);
        lengthInflections.Add(int.MaxValue);
        var nextLenInflection = lengthInflections[0];
        lengthInflections.RemoveAt(0);

        var velocityInflections = RandomHelper.Points(0, numberOfLines, velocityInflectionCount);
        velocityInflections.Add(int.MaxValue);
        var nextVelocityInflection = velocityInflections[0];
        velocityInflections.RemoveAt(0);

        var gapInflections = RandomHelper.Points(0, numberOfLines, numberOfLargeGaps);
        gapInflections.Add(int.MaxValue);
        var nextGapInflection = gapInflections[0];
        gapInflections.RemoveAt(0);

        var currXPos = -0.5f * (CameraHelper.Width + equalizerWidth);
        var yPos = (CameraHelper.Height-baseLength*2) * Random.value - CameraHelper.HalfWidth;

        var currLenDir= 1;
        var currLen = baseLength;
        var currVelocityDir = 1;
        var currVelocity = baseVelocity;

        for (int i = 0; i < numberOfLines; i++) {
            var line = new LineParams();
            line.length = currLen;
            line.width = baseWidth;
            line.x = currXPos;
            line.y = yPos;
            line.velocity = currVelocity; // for now, same velocity

            lines.Add(line);

            // next
            currXPos += baseWidth + baseGap;
            currLen += currLenDir * lengthIncrement;
            currVelocity += velocityIncrement * currVelocityDir;

            if (nextLenInflection <= i) {
                // reverse dir
                currLenDir *= -1;
                nextLenInflection =lengthInflections[0];
                lengthInflections.RemoveAt(0);
            }

            if (nextVelocityInflection <= i) {
                // reverse dir
                currVelocityDir *= -1;
                nextVelocityInflection = velocityInflections[0];
                velocityInflections.RemoveAt(0);
            }

            if (nextGapInflection <= i) {
                // reverse dir
                currXPos += largeGap;
                nextGapInflection = gapInflections[0];
                gapInflections.RemoveAt(0);
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
}




