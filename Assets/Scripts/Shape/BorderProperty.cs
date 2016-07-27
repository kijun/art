using UnityEngine;
using System;

[System.Serializable]
public struct BorderProperty : IEquatable<BorderProperty> {
//public struct BorderProperty : IEquatable<BorderProperty> {
    public BorderStyle style;
    public Color color;
    public float thickness;
    public BorderPosition position;
    public float dashLength;
    public float gapLength;

    public BorderProperty(
            BorderStyle     style = BorderStyle.None,
            BorderPosition  position = BorderPosition.Center,
            Color           color = new Color(),
            float           thickness = 0,
            float           dashLength = 0.05f,
            float           gapLength = 0.05f
    ) {
        this.style = style;
        this.position = position;
        this.color = color;
        this.thickness = thickness;
        this.dashLength = dashLength;
        this.gapLength = gapLength;
    }

    public bool MeshNeedsUpdate(BorderProperty other) {
        if (style != other.style) return true;

        // borderless - no update
        if (style == BorderStyle.None) return false;

        // thickness or border position changed - update
        if (!Mathf.Approximately(thickness, other.thickness) || position != other.position) {
            return true;
        } else {
        }

        // dashed border & gap/length changed - update
        if (style == BorderStyle.Dash &&
            (!Mathf.Approximately(dashLength, other.dashLength) ||
             !Mathf.Approximately(gapLength, other.gapLength))) {
            return true;
        }

        return false;
    }

    /*
     * Comparators
     */
    public override bool Equals(object other) {
        if (other is BorderProperty) {
            return Equals((BorderProperty)other);
        }
        return false;
    }

    public bool Equals(BorderProperty other) {

        // Performance?
        if (style == other.style &&
            color == other.color &&
            position == other.position &&
            Mathf.Approximately(thickness, other.thickness) &&
            Mathf.Approximately(dashLength, other.dashLength) &&
            Mathf.Approximately(gapLength, other.gapLength))
        {
            return true;
        }
        return false;
    }

    public static bool operator ==(BorderProperty p1, BorderProperty p2) {
        return p1.Equals(p2);
    }

    public static bool operator !=(BorderProperty p1, BorderProperty p2) {
        return !p1.Equals(p2);
    }

    public override int GetHashCode() {
        // TODO this should be alright
        int hash = 13;
        hash = (hash * 7) + GetType().GetHashCode();
        hash = (hash * 7) + position.GetHashCode();
        hash = (hash * 7) + color.GetHashCode();
        hash = (hash * 7) + thickness.GetHashCode();
        hash = (hash * 7) + dashLength.GetHashCode();
        hash = (hash * 7) + gapLength.GetHashCode();
        return hash;
    }
}
