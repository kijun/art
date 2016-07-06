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
}

