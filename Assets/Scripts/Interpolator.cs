using UnityEngine;
using System.Collections;

public static class Interpolator {
    // converts time to ratio and also gives . basic lerp and stuff good.
    public static float Lerp(Range range, float progress) {
        progress = Mathf.Clamp(progress, 0f, 1f);
        return progress * (range.maximum - range.minimum) + range.minimum;
    }

    public static Vector2 UnitVectorWithAngle(float angleInDegrees) {
        var angleInRadian = angleInDegrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angleInRadian), Mathf.Sin(angleInRadian));
    }

    public static Vector2 Lerp(Vector2 start, Vector2 end, float progress) {
        return Vector3.Lerp(start, end, progress);
    }

    public static Vector2 Bezier(Vector2 start, Vector2 end, float progress) {
        var bez = new Bezier(Vector2.zero, new Vector2(0.1f, 0.04f), new Vector2(0f, 1f), Vector2.one);
        var point = bez.GetPoint(Mathf.Clamp(progress, 0, 1));
        Debug.Log(point.x.ToString("0.00") + ", " + point.y.ToString("0.00"));
        var final = Lerp(start, end, Mathf.Clamp(point.y, 0, 1));
        Debug.Log("so it should be at" + final.ToString("0.000"));
        return final;
    }
}
