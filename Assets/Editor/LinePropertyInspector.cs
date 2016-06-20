using UnityEngine;
using System.Collections;
using UnityEditor;

//[CustomEditor(typeof(LineProperty))]
public class LinePropertyInspector : Editor {
//    public override void OnInspectorGUI() {
//        //LineProperty line = target as LineProperty;
//        LineProperty line = new LineProperty();
//
//        EditorGUI.BeginChangeCheck();
//
//        // Width, Length in pixels
//        line.length = EditorGUILayout.FloatField("Length", line.length * 100f) / 100f;
//        line.width = EditorGUILayout.FloatField("Width", line.width * 100f) / 100f;
//        line.angle = EditorGUILayout.FloatField("Angle", line.angle);
//
//        // Color
//        line.color = EditorGUILayout.ColorField("Color", line.color);
//
//        // Line Style
//        line.style = (BorderStyle)EditorGUILayout.EnumPopup("Style", line.style);
//
//        if (line.style == BorderStyle.Dash) {
//            line.dashLength = EditorGUILayout.FloatField("Dash Length", line.dashLength * 100f) / 100f;
//            line.gapLength = EditorGUILayout.FloatField("Gap Length", line.gapLength * 100f) / 100f;
//        }
//
//        // Render
//        /*
//        if (EditorGUI.EndChangeCheck()) {
//            line.OnUpdate();
//        }
//        */
//    }
//
//
//    /*
//    void OnSceneGUI() {
//        var line = target as LineProperty;
//        if (line == null) return;
//
//        // point 1
//        Vector2 p1 = Handles.FreeMoveHandle(line.EndPoint1, Quaternion.identity, 0.1f, new Vector2(0.01f, 0.01f), Handles.DotCap);
//        if (p1 != line.EndPoint1) {
//            line.EndPoint1 = p1;
//        }
//
//        // point 2
//        Vector2 p2 = Handles.FreeMoveHandle(line.EndPoint2, Quaternion.identity, 0.1f, new Vector2(0.01f, 0.01f), Handles.DotCap);
//        if (p2 != line.EndPoint2) {
//            line.EndPoint2 = p2;
//        }
//
//        // rotation handle
//        float handleSize = HandleUtility.GetHandleSize(line.transform.position);
//        Quaternion rot = Handles.Disc(
//                Quaternion.Euler(0, 0, line.Angle),
//                line.transform.position,
//                Vector3.forward,
//                0.5f * handleSize,
//                false,
//                0.1f);
//
//        float newAngle = rot.eulerAngles.z;
//        if (Mathf.Abs(newAngle - line.Angle) > float.Epsilon) {
//            //Debug.Log(newAngle + " | " + line.Angle);
//            line.Angle = newAngle;
//        }
//    }
//    */
}
