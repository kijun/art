using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

[CustomEditor(typeof(RectRenderer))]
public class RectPropertyInspector : Editor {

    public override void OnInspectorGUI() {
        var renderer = (RectRenderer)target;
        var rect = renderer.property.Clone() as RectProperty;

        EditorGUI.BeginChangeCheck();

        // Width, Length in pixels
        rect.height = EditorGUILayout.FloatField("Height", rect.height * 100f) / 100f;
        rect.width = EditorGUILayout.FloatField("Width", rect.width * 100f) / 100f;

        rect.angle = EditorGUILayout.FloatField("Angle", rect.angle);

        rect = (RectProperty)ShapePropertyInspector.Inspect(rect);

        // Render
        if (EditorGUI.EndChangeCheck()) {
            renderer.property = rect;
            renderer.RenderAndUpdatePropertyIfNeeded();
            EditorUtility.SetDirty(renderer);
        }
    }
//
//    void OnSceneGUI() {
//        var obj = target as RectProperty;
//        if (obj == null) {
//            return;
//        }
//        Rect2 rect = obj.rect2;
//        float handleSize = HandleUtility.GetHandleSize(obj.Center);
//
//        // resize handles
//        foreach (Direction dir in Enum.GetValues(typeof(Direction))) {
//            var anchor = rect.Point(dir);
//
//            Vector2 newAnchor = Handles.FreeMoveHandle(anchor, Quaternion.identity, 0.05f, new Vector2(0.1f, 0.1f), Handles.DotCap);
//            if (Vector2.Distance(anchor, newAnchor) > float.Epsilon) {
//                //Debug.Log("--Prev--");
//                //Debug.Log(rect);
//                rect.MovePoint(dir, newAnchor);
//                //Debug.Log("--Changed--");
//                //Debug.Log(rect);
//                obj.rect2 = rect;
//            }
//        }
//
//        // rotate handle
//        Quaternion rot = Handles.Disc(
//                Quaternion.Euler(0, 0, obj.Angle),
//                obj.Center,
//                Vector3.forward,
//                0.5f * handleSize,
//                false,
//                5f);
//
//        float newAngle = rot.eulerAngles.z;
//        if (Mathf.Abs(newAngle - obj.Angle) > float.Epsilon) {
//            //Debug.Log(newAngle + " | " + line.Angle);
//            obj.Angle = newAngle;
//        }
//    }
}

