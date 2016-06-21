using UnityEngine;

// TODO remove interface
[System.Serializable]
public class CircleProperty : ShapeProperty {
    public float diameter;

    public CircleProperty(
            Vector2         center = new Vector2(),
            float           diameter = 1,
            float           angle = 0,
            Color           color = new Color(),
            BorderProperty  border= new BorderProperty()
    ) : base(center:center, angle:angle, color:color, border:border) {
        this.diameter = diameter;
    }

    /* ShapeProperty */

    public override Vector2 scale {
        get { return new Vector2(diameter, diameter); }
    }
}

