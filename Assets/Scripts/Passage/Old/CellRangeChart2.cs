using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CellRangeChart2 : BaseCell {
    public int numberOfLines;

    public float baseLength;
    public float lengthIncrement;

    public Vector2 baseVelocity;
    public Vector2 velocityIncrement;

    public float baseWidth;
    public float baseGap;
    public float largeGap;

    public float p_gap;
    public float p_lengthInflection;
    public float p_velocityInflection;

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

        var equalizerWidth = numberOfLines * baseWidth + (numberOfLines - 1) * baseGap + largeGap * p_gap * 1.5f * numberOfLines;

        var currXPos = -0.5f * (CameraHelper.Width ) - equalizerWidth;
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

            if (Random.value < p_lengthInflection) { currLenDir *= -1; }

            if (Random.value < p_velocityInflection) { currVelocityDir *= -1; }

            if (Random.value < p_gap) { currXPos += largeGap; }
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





