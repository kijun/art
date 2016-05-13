using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CircleProperty))]
public class CirclePropertyInspector : Editor {

    const float MIN_RADIUS_CHANGE = 0.1f;

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
            circle.borderPosition = (BorderPosition)EditorGUILayout.EnumPopup("Border Position", circle.borderPosition);
            circle.borderThickness = EditorGUILayout.FloatField("Border Thickness",
                    circle.borderThickness);
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

    void OnSceneGUI() {
        var obj = target as CircleProperty;

        Vector2 center = Handles.FreeMoveHandle(
                obj.center,
                Quaternion.identity,
                0.1f,
                new Vector2(0.01f, 0.01f), // handle size
                Handles.DotCap);

        if (center != obj.center) {
            obj.center = center;
        }

        Vector2 radiusAnchor = Handles.FreeMoveHandle(
                obj.center + new Vector2(obj.diameter/2, 0),
                Quaternion.identity,
                0.1f,
                new Vector2(0.01f, 0.01f), // handle size
                Handles.DotCap);

        float diameter = 2*Vector2.Distance(radiusAnchor, obj.center);
        if (Mathf.Abs(diameter - obj.diameter) > MIN_RADIUS_CHANGE) {
            obj.diameter = diameter;
            obj.OnUpdate();
        } else {
            //Debug.Log(diameter + " " + obj.diameter);
        }
    }
}
