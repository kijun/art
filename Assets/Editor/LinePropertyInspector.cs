using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LineProperty))]
public class LinePropertyInspector : Editor {
    public override void OnInspectorGUI() {
        LineProperty line = (LineProperty)target;

        EditorGUI.BeginChangeCheck();

        // Width, Length in pixels
        line.Length = EditorGUILayout.FloatField("Length", line.Length * 100f) / 100f;
        line.Width = EditorGUILayout.FloatField("Width", line.Width * 100f) / 100f;
        line.Angle = EditorGUILayout.FloatField("Angle", line.Angle);

        // Color
        line.color = EditorGUILayout.ColorField("Color", line.color);

        // Line Style
        line.style = (BorderStyle)EditorGUILayout.EnumPopup("Style", line.style);

        if (line.style == BorderStyle.Dash) {
            line.dashLength = EditorGUILayout.FloatField("Dash Length", line.dashLength * 100f) / 100f;
            line.gapLength = EditorGUILayout.FloatField("Gap Length", line.gapLength * 100f) / 100f;
        }

        // Render
        if (EditorGUI.EndChangeCheck()) {
            line.OnPropertyChange();
        }
    }
}
