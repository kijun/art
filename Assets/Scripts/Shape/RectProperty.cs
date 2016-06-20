using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

// execute in edit mode
[ExecuteInEditMode]
[SelectionBase]

public struct RectProperty : IShapeProperty, IEquatable<RectProperty> {

    /*
     * ATTRIBUTES
     */
    Vector2 _center;
    float _angle;
    public Color _color;
    BorderProperty _borderProperty;
    public float height;
    public float width;

    public RectProperty(
            Vector2         center = new Vector2(),
            float           height = 1,
            float           width = 1,
            float           angle = 0,
            Color           color = new Color(),
            BorderProperty  borderProperty = new BorderProperty()
    ) {
        _center = center;
        this.height = height;
        this.width = width;
        _angle = angle;
        _color = color;
        _borderProperty = borderProperty;
    }

    /* IShapeProperty */

    public override Vector2 scale {
        get { return new Vector2(width, height); }
    }

    public override Vector2 center {
        get { return _center; }
        set { _center = value; }
    }

    public override float angle {
        get { return _angle; }
        set { _angle = value; }
    }

    public override Color color {
        get { return _color; }
        set { _color = value; }
    }

    public override BorderProperty borderProperty {
        get { return _borderProperty; }
        set { _borderProperty = value; }
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
}
