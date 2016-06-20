using UnityEngine;
using System;

public struct BorderProperty : IEquatable<BorderProperty> {
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
        }

        // dashed border & gap/length changed - update
        if (style == BorderStyle.Dash &&
            (!Mathf.Approximately(dashLength, other.dashLength) ||
             !Mathf.Approximately(gapLength, other.gapLength))) {
            return true;
        }

        return false;
    }
}
