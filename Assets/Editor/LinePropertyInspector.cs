using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LineProperty))]
public class LinePropertyInspector : Editor {
    public override void OnInspectorGUI() {
        LineProperty line = (LineProperty)target;
        Transform lineTransform = line.transform;

        EditorGUI.BeginChangeCheck();

        // Width, Length in pixels
        Vector3 newScale = lineTransform.localScale;
        newScale = new Vector3(
                EditorGUILayout.FloatField("Length", lineTransform.localScale.x*100f) / 100f,
                EditorGUILayout.FloatField("Width", lineTransform.localScale.y*100f) / 100f,
                newScale.z);
        lineTransform.localScale = newScale;

        // Angle in degrees
        Vector3 eulerRot = lineTransform.eulerAngles;
        eulerRot = eulerRot.SwapZ(
                    EditorGUILayout.FloatField("Angle", lineTransform.eulerAngles.z));
        lineTransform.eulerAngles = eulerRot;

        // Color
        /*
        line.color = EditorGUILayout.ColorField("Color", line.color);
        line.
        */

        // Line Style
        line._style = (BorderStyle)EditorGUILayout.EnumPopup("Style", line._style);

        if (line._style == BorderStyle.Dash) {
            line._dashLength = EditorGUILayout.FloatField("Dash Length", line._dashLength);
            line._gapLength = EditorGUILayout.FloatField("Gap Length", line._gapLength);
        }

        // Render
        if (EditorGUI.EndChangeCheck()) {
            line.OnPropertyChange();
        }
    }
}
