using UnityEngine;


[System.Serializable]
public class LineProperty : ShapeProperty {

    public float length;
    public float width;

    public LineProperty(
            Vector2         center = new Vector2(),
            float           length = 1,
            float           width = 1,
            float           angle = 0,
            Color           color = new Color(),
            BorderProperty  border = new BorderProperty()
    ) : base(shapeType: ShapeType.Line, center:center, angle:angle, color:color, border:border) {
        this.length = length;
        this.width = width;
    }

    /* ShapeProperty */

    public override Vector2 scale {
        get { return new Vector2(length, width); }
    }

    public override object Clone() {
        return new LineProperty(center:center, length:length, width:width, angle:angle, color:color, border:border);
    }

    /* Generated Properties */
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
}

