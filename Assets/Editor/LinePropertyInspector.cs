using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LineProperty))]
public class LinePropertyInspector : Editor {
    public override void OnInspectorGUI() {
        LineProperty line = target as LineProperty;

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
            line.OnUpdate();
        }
    }


    void OnSceneGUI() {
        var line = target as LineProperty;
        Vector2 p1 = Handles.FreeMoveHandle(line.EndPoint1, Quaternion.identity, 0.1f, new Vector2(0.01f, 0.01f), Handles.DotCap);
        if (p1 != line.EndPoint1) {
            Debug.Log("New Endpoint " + p1);
            line.EndPoint1 = p1;
        }
        Vector2 p2 = Handles.FreeMoveHandle(line.EndPoint2, Quaternion.identity, 0.1f, new Vector2(0.01f, 0.01f), Handles.DotCap);
        if (p2 != line.EndPoint2) {
            Debug.Log("New Endpoint " + p2);
            line.EndPoint2 = p2;
        }
    }
}
