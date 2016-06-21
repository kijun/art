using UnityEditor;
using UnityEngine;
using System.Collections;

public static class ShapePropertyInspector {
    public static ShapeProperty Inspect(ShapeProperty shape) {
        shape.color = EditorGUILayout.ColorField("Color", shape.color);

        var border = shape.border;

        // Circle Style
        border.style = (BorderStyle)EditorGUILayout.EnumPopup("Style", border.style);

        if (border.style != BorderStyle.None) {
            border.color = EditorGUILayout.ColorField("Border Color", border.color);
            border.position = (BorderPosition)EditorGUILayout.EnumPopup("Border Position", border.position);
            border.thickness = EditorGUILayout.FloatField("Border Thickness",
                    border.thickness * 100f) / 100f;
            if (border.style == BorderStyle.Dash) {
                border.dashLength = EditorGUILayout.FloatField("Dash Length", border.dashLength);
                border.gapLength = EditorGUILayout.FloatField("Gap Length", border.gapLength);
            }
        }

        shape.border = border;

        return shape;
    }
}
