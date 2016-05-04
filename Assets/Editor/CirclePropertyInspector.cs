using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CircleProperty))]
public class CirclePropertyInspector : Editor {
    public override void OnInspectorGUI() {
        CircleProperty circle = (CircleProperty)target;
        Transform circleTransform = circle.transform;

        EditorGUI.BeginChangeCheck();

        // Width, Length in pixels
        //
        circle.diameter = EditorGUILayout.FloatField("Diameter", circle.diameter);


        // Color
        circle.color = EditorGUILayout.ColorField("Color", circle.color);

        // Circle Style
        circle.style = (BorderStyle)EditorGUILayout.EnumPopup("Style", circle.style);

        if (circle.style != BorderStyle.None) {
            circle.borderColor = EditorGUILayout.ColorField("Border Color", circle.borderColor);
            circle.borderWidth = EditorGUILayout.FloatField("Border Width", circle.borderWidth);
            if (circle.style == BorderStyle.Dash) {
                circle.dashLength = EditorGUILayout.FloatField("Dash Length", circle.dashLength);
                circle.gapLength = EditorGUILayout.FloatField("Gap Length", circle.gapLength);
            }
        }

        // Render
        if (EditorGUI.EndChangeCheck()) {
            circle.OnPropertyChange();
        }
    }
}
