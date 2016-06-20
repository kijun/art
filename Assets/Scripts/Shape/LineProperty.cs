using UnityEngine;
using System;
using System.Collections;


// TODO remove interface!
[Serializable]
public struct LineProperty : IShapeProperty, IEquatable<LineProperty> {
    public Vector2 center;
    public float length;
    public float angle;
    public float width;
    public Color color;
    public BorderStyle style;
    public float dashLength;
    public float gapLength;

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
        this.length = length;
        this.color = color;
        this.width = width;
        this.style = style;
        this.dashLength = dashLength;
        this.gapLength = gapLength;
    }

    // let's keep p1 p2 as the default data but also
    // provide a constructor for
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
            var pts = TupleUtil.Sort(value);
            var pt1 = pts.Item1;
            var pt2 = pts.Item2;

            center = (pt1+pt2) / 2f;
            length = (pt2-pt1).magnitude;
            angle = Mathf.Atan2(p2.y-p1.y, p2.x-p1.x) * Mathf.Rad2Deg;
        }
    }


    /* Equality check */
    public override bool Equals(object other) {
        if (other is LineProperty) {
            return Equals((LineProperty)other);
        }
        return false;
    }


    public bool Equals(LineProperty other) {
        // TODO compare everything...
        if (Mathf.Approximately(length, other.length) &&
            Mathf.Approximately(width, other.width) &&
            Mathf.Approximately(angle, other.angle) &&
            center == other.center) {
            return true;
        }
        return false;
    }

    public static bool operator ==(LineProperty p1, LineProperty p2) {
        return p1.Equals(p2);
    }

    public static bool operator !=(LineProperty p1, LineProperty p2) {
        return !p1.Equals(p2);
    }

    public override int GetHashCode() {
        // TODO this should be alright
        int hash = 13;
        hash = (hash * 7) + length.GetHashCode();
        hash = (hash * 11) + width.GetHashCode();
        hash = (hash * 17) + angle.GetHashCode();
        hash = (hash * 23) + center.GetHashCode();
        return hash;
    }

    /*
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
    */
}

