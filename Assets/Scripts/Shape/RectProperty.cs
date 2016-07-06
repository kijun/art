using UnityEngine;

[System.Serializable]
public class RectProperty : ShapeProperty {

    public float height;
    public float width;

    public RectProperty(
            Vector2         center = new Vector2(),
            float           height = 1,
            float           width = 1,
            float           angle = 0,
            Color           color = new Color(),
            BorderProperty  border = new BorderProperty()
    ) : base(shapeType:ShapeType.Rect, center:center, angle:angle, color:color, border:border) {
        this.height = height;
        this.width = width;
    }

    /* ShapeProperty */

    public override Vector2 scale {
        get { return new Vector2(width, height); }
    }
}
