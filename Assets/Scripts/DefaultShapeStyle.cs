using UnityEngine;
using UnityEditor;
using System.Collections;


public class DefaultShapeStyle {
    /*
     * BASIC PROPERTIES
     */
    public static float lineLength = 1f;
    public static float lineWidth = 1f;

    public static float rectHeight = 1f;
    public static float rectWidth = 1f;

    public static float diameter = 1f;

    // shared property
    // TODO reference palette
    public static Color color = Color.black;
    public static float angle = 0;

    /*
     * BORDER PROPERTIES
     */
    public static BorderStyle borderStyle = BorderStyle.None;
    // this is also shared by line shape as well
    public static float borderThickness = 0.05f;
    public static Color borderColor = Color.grey;
    public static BorderPosition borderPosition = BorderPosition.Outside;
    public static float dashLength = 0.10f;
    public static float gapLength = 0.10f;

    // TODO should probably return a line property struct,
    // and we should refactor rendering code
    public static void ApplyToLine(LineProperty line) {
        line.color = color;
        line.style = borderStyle;
        line.dashLength = dashLength;
        line.gapLength = gapLength;
        line.Angle = angle;
        line.Length = lineLength;
        line.Width = lineWidth;
    }

    public static void SetDefaultLineStyle(LineProperty line) {
        color = line.color;
        borderStyle = line.style;
        dashLength = line.dashLength;
        gapLength = line.gapLength;
        angle = line.Angle;
        lineLength = line.Length;
        lineWidth = line.Width;
    }

    public static void ApplyToRect(RectProperty obj) {
        obj.color = color;
        obj.Height = rectHeight;
        obj.Width = rectWidth;
        obj.Angle = angle;

        obj.borderStyle = borderStyle;
        obj.borderPosition = borderPosition;
        obj.dashLength = dashLength;
        obj.gapLength = gapLength;
    }

    public static void SetDefaultRectStyle(RectProperty obj) {
        color = obj.color;
        rectHeight = obj.Height;
        rectWidth = obj.Width;
        angle = obj.Angle;

        borderStyle = obj.borderStyle;
        borderPosition = obj.borderPosition;
        dashLength = obj.dashLength;
        gapLength = obj.gapLength;
    }

    public static void Apply(IObjectProperty p) {
        if (p.GetType() == typeof(LineProperty)) {
            LineProperty lp = (LineProperty)p;
            ApplyToLine(lp);
        } else if (p.GetType() == typeof(RectProperty)) {
            ApplyToRect((RectProperty)p);
        }
    }
}
