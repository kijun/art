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
        circle.borderStyle = (BorderStyle)EditorGUILayout.EnumPopup("Style", circle.borderStyle);

        if (circle.borderStyle != BorderStyle.None) {
            circle.borderColor = EditorGUILayout.ColorField("Border Color", circle.borderColor);
            circle.borderWidth = EditorGUILayout.FloatField("Border Width", circle.borderWidth);
            if (circle.borderStyle == BorderStyle.Dash) {
                circle.dashLength = EditorGUILayout.FloatField("Dash Length", circle.dashLength);
                circle.gapLength = EditorGUILayout.FloatField("Gap Length", circle.gapLength);
            }
        }

        // MeshFilter/Renderer
        circle.innerMeshRenderer = (MeshRenderer)EditorGUILayout.ObjectField(
                "Inner Mesh Renderer",
                circle.innerMeshRenderer,
                typeof(MeshRenderer),
                true);
        circle.innerMeshFilter = (MeshFilter)EditorGUILayout.ObjectField(
                "Inner Mesh Filter",
                circle.innerMeshFilter,
                typeof(MeshFilter),
                true);

        circle.borderMeshRenderer = (MeshRenderer)EditorGUILayout.ObjectField(
                "Border Mesh Renderer",
                circle.borderMeshRenderer,
                typeof(MeshRenderer),
                true);
        circle.borderMeshFilter = (MeshFilter)EditorGUILayout.ObjectField(
                "Border Mesh Filter",
                circle.borderMeshFilter,
                typeof(MeshFilter),
                true);

        // Render
        if (EditorGUI.EndChangeCheck()) {
            circle.OnUpdate();
        }
    }
}
