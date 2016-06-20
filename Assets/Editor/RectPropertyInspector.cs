using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

//[CustomEditor(typeof(RectProperty))]
public class RectPropertyInspector : Editor {
//    public override void OnInspectorGUI() {
//        RectProperty rect = (RectProperty)target;
//        Transform rectTransform = rect.transform;
//
//        EditorGUI.BeginChangeCheck();
//
//        // Width, Length in pixels
//        rect.Height = EditorGUILayout.FloatField("Height", rect.Height * 100f) / 100f;
//        rect.Width = EditorGUILayout.FloatField("Width", rect.Width * 100f) / 100f;
//
//        rect.Angle = EditorGUILayout.FloatField("Angle", rect.Angle);
//
//        // Color
//        rect.color = EditorGUILayout.ColorField("Color", rect.color);
//
//        // Rect Style
//        rect.borderStyle = (BorderStyle)EditorGUILayout.EnumPopup("Border Style", rect.borderStyle);
//
//        // Shared Border Property
//        if (rect.borderStyle != BorderStyle.None) {
//            rect.borderPosition = (BorderPosition)EditorGUILayout.EnumPopup("Border Position", rect.borderPosition);
//            rect.borderColor = EditorGUILayout.ColorField("Border Color", rect.borderColor);
//            rect.borderThickness = EditorGUILayout.FloatField("Border Thickness", rect.borderThickness * 100f) / 100f;
//        }
//
//        // Dash Border Property
//        if (rect.borderStyle == BorderStyle.Dash) {
//            rect.dashLength = EditorGUILayout.FloatField("Dash Length", rect.dashLength * 100f) / 100f;
//            rect.gapLength = EditorGUILayout.FloatField("Gap Length", rect.gapLength * 100f) / 100f;
//        }
//
//        // MeshFilter/Renderer
//        rect.innerMeshRenderer = (MeshRenderer)EditorGUILayout.ObjectField(
//                "Inner Mesh Renderer",
//                rect.innerMeshRenderer,
//                typeof(MeshRenderer),
//                true);
//        rect.innerMeshFilter = (MeshFilter)EditorGUILayout.ObjectField(
//                "Inner Mesh Filter",
//                rect.innerMeshFilter,
//                typeof(MeshFilter),
//                true);
//
//        rect.borderMeshRenderer = (MeshRenderer)EditorGUILayout.ObjectField(
//                "Border Mesh Renderer",
//                rect.borderMeshRenderer,
//                typeof(MeshRenderer),
//                true);
//        rect.borderMeshFilter = (MeshFilter)EditorGUILayout.ObjectField(
//                "Border Mesh Filter",
//                rect.borderMeshFilter,
//                typeof(MeshFilter),
//                true);
//
//        // Render
//        if (EditorGUI.EndChangeCheck()) {
//            //rect.OnPropertyChange();
//            rect.Render();
//        }
//    }
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

