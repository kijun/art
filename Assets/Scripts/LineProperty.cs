using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;


public interface IObjectProperty {
    void OnUpdate();
}

public struct LineProperty {
    public Vector2 center;
    public float length;
    public float angle;

    public float width;

    public Color color;
    public BorderStyle style;
    public float dashLength;
    public float gapLength;

    // let's keep p1 p2 as the default data but also
    // provide a constructor for
    //
    //
    public Vector2 p1 {
        get {
            return points.Item1;
        }
        set {
            points = new Tuple<Vector2, Vector2>(value, p2);
        }
    }

    public Vector2 p2 {
        get {
            return points.Item2;
        }
        set {
            points = new Tuple<Vector2, Vector2>(p1, value);
        }
    }

    public Tuple<Vector2, Vector2> points {
        get {
            Vector2 directional = Quaternion.Euler(0, 0, angle) * new Vector2(length/2f, 0);
            var pt1 = directional + center;
            var pt2 = -1*directional + center;
            return new Tuple<Vector2, Vector2>(pt1, pt2);
        }
        set {
            var pts = TupleHelper.Sort((Tuple<Vector2, Vector2>)value);
            var pt1 = pts.Item1;
            var pt2 = pts.Item2;

            center = (pt1+pt2) / 2f;
            length = (pt2-pt1).magnitude;
            angle = Mathf.Atan2(p2.y-p1.y, p2.x-p1.x) * Mathf.Rad2Deg;
        }
    }

    // default
    public LineProperty(
            Vector2         center = new Vector2(),
            float           angle = 0,
            float           length = 1,
            float           width = 0.1f,
            Color           color = new Color(),
            BorderStyle     style = BorderStyle.None,
            float           dashLength = 0.05f,
            float           gapLength = 0.05f
    ) {
        this.center = center;
        this.angle = angle;
        this.length = 1;
        this.color = color;
        this.width = width;
        this.style = style;
        this.dashLength = dashLength;
        this.gapLength = gapLength;
    }

    public LineProperty(
            Vector2         p1 = new Vector2(),
            Vector2         p2 = new Vector2(),
            float           width = 0.1f,
            Color           color = new Color(),
            BorderStyle     style = BorderStyle.None,
            float           dashLength = 0.05f,
            float           gapLength = 0.05f
    ) {
        this.width = width;
        this.style = style;
        this.dashLength = dashLength;
        this.gapLength = gapLength;

        this.points = new Tuple<Vector2, Vector2>(p1, p2);
    }
}

