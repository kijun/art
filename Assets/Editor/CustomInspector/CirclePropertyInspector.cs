using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(CircleRenderer))]
public class CirclePropertyInspector : Editor {

    const float MIN_RADIUS_CHANGE = 0.1f;

    public override void OnInspectorGUI() {
        var circleR = (CircleRenderer)target;
        var circle = circleR.property.Clone() as CircleProperty;

        EditorGUI.BeginChangeCheck();

        // Width, Length in pixels
        circle.diameter = EditorGUILayout.FloatField("Diameter", circle.diameter);

        circle = (CircleProperty)ShapePropertyInspector.Inspect(circle);

        // Render
        if (EditorGUI.EndChangeCheck()) {
            circleR.property = circle;
            circleR.RenderAndUpdatePropertyIfNeeded();
            EditorUtility.SetDirty(circleR);
        }
    }

    /*
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
    */
}
