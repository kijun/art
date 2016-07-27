using UnityEngine;

// TODO remove interface
[System.Serializable]
public class CircleProperty : ShapeProperty {
    public float diameter;

    public CircleProperty(
            Vector2         center = new Vector2(),
            float           diameter = 1,
            Color           color = new Color(),
            BorderProperty  border= new BorderProperty()
    ) : base(shapeType: ShapeType.Circle, center:center, angle:0, color:color, border:border) {
        this.diameter = diameter;
    }

    /* ShapeProperty */

    public override Vector2 scale {
        get { return new Vector2(diameter, diameter); }
    }

    public override object Clone() {
        return new CircleProperty(center:center, diameter:diameter, color:color, border:border);
    }

    public float innerCircleDiameter {
        get {
            return diameter - 2*border.thickness;
        }
        set {
            Debug.Log("dia" + diameter);
            Debug.Log(value);
            border.thickness = (diameter - value)/2f;
        }
    }
    // TODO:comparator belongs here
}

