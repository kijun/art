using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

// TODO remove interface
public struct CircleProperty : IShapeProperty, IEquatable<CircleProperty> {
    public Vector2 center;
    public float diameter;
    public Color color;

    // border
    public BorderStyle borderStyle;
    public BorderPosition borderPosition;
    public Color borderColor;
    public float borderThickness;
    public float dashLength;
    public float gapLength;

    public CircleProperty(
            Vector2         center = new Vector2(),
            float           diameter = 1,
            Color           color = new Color(),
            BorderStyle     borderStyle = BorderStyle.None,
            BorderPosition  borderPosition = BorderPosition.Center,
            Color           borderColor = new Color(),
            float           borderThickness = 0,
            float           dashLength = 0.05f,
            float           gapLength = 0.05f
    ) {
        this.diameter = diameter;
        this.center = center;
        this.color = color;
        this.borderStyle = borderStyle;
        this.borderPosition = borderPosition;
        this.borderThickness = borderThickness;
        this.borderColor = borderColor;
        this.dashLength = dashLength;
        this.gapLength = gapLength;
    }

    /*
     * Comparator
     */

    public override bool Equals(object other) {
        if (other is CircleProperty) {
            return Equals((CircleProperty)other);
        }
        return false;
    }

    public bool Equals(CircleProperty other) {
        // TODO compare everything...
        if (center == other.center &&
            Mathf.Approximately(diameter, other.diameter) &&
            // TODO color equality
            borderStyle == other.borderStyle &&
            borderPosition == other.borderPosition &&
            // TODO border color equality
            Mathf.Approximately(borderThickness, other.borderThickness) &&
            Mathf.Approximately(dashLength, other.dashLength) &&
            Mathf.Approximately(gapLength, other.gapLength)) {
            return true;
        }
        return false;
    }

    public static bool operator ==(CircleProperty p1, CircleProperty p2) {
        return p1.Equals(p2);
    }

    public static bool operator !=(CircleProperty p1, CircleProperty p2) {
        return !p1.Equals(p2);
    }

    public override int GetHashCode() {
        // TODO this should be alright
        int hash = 13;
        hash = (hash * 7) + center.GetHashCode();
        hash = (hash * 11) + diameter.GetHashCode();
        hash = (hash * 17) + color.GetHashCode();
        hash = (hash * 23) + borderStyle.GetHashCode();
        return hash;
    }
}

