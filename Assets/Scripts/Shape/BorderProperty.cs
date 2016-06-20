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
}
