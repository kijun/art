using UnityEngine;
using System;

//public interface IShapeProperty {
[System.Serializable]
public abstract class ShapeProperty : IEquatable<ShapeProperty>, ICloneable {

    /*
     * Shared Attributes
     */
    public Vector2 center;
    public float angle;
    public Color color;
    public BorderProperty border;
    public ShapeType shapeType;

    /*
     * Constructor
     */

    protected ShapeProperty(
            ShapeType       shapeType,
            Vector2         center = new Vector2(),
            float           angle = 0,
            Color           color = new Color(),
            BorderProperty  border = new BorderProperty()
    ) {
        this.shapeType = shapeType;
        this.center = center;
        this.angle = angle;
        this.color = color;
        this.border = border;
    }

    /*
     * Properties
     */

    public abstract Vector2 scale {
        get;
    }

    public abstract object Clone();

    /*
     * Comparators
     */
    public override bool Equals(object other) {
        if (other is ShapeProperty) {
            return Equals((ShapeProperty)other);
        }
        return false;
    }

    public bool Equals(ShapeProperty other) {
        // different shape property classes
        if (this.GetType() != other.GetType()) return false;

        // Performance?
        if (center == other.center &&
            scale == other.scale &&
            Mathf.Approximately(angle, other.angle) &&
            border == other.border) { //?
            return true;
        }
        return false;
    }

    public static bool operator ==(ShapeProperty p1, ShapeProperty p2) {
        return p1.Equals(p2);
    }

    public static bool operator !=(ShapeProperty p1, ShapeProperty p2) {
        return !p1.Equals(p2);
    }

    public override int GetHashCode() {
        // TODO this should be alright
        int hash = 13;
        hash = (hash * 7) + GetType().GetHashCode();
        hash = (hash * 11) + center.GetHashCode();
        hash = (hash * 17) + scale.GetHashCode();
        hash = (hash * 23) + angle.GetHashCode();
        hash = (hash * 23) + border.GetHashCode();
        return hash;
    }

}
