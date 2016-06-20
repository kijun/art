using UnityEngine;
using UnityEditor;
using System.Collections;

public class ObjectToolbar : EditorWindow {
//
//    const string LINE_PREFAB_PATH = "Shapes/UnitLine";
//    const string RECT_PREFAB_PATH = "Shapes/UnitRect";
//    const string CIRCLE_PREFAB_PATH = "Shapes/UnitCircle";
//
//	[MenuItem ("Window/Object Toolbar")]
//	static void Init () {
//		// Get existing open window or if none, make a new one:
//		ObjectToolbar window = (ObjectToolbar)EditorWindow.GetWindow(
//                typeof (ObjectToolbar));
//	}
//
//    [MenuItem ("GameObject/Line %&l")]
//    static void CreateLine() {
//        Create<LineProperty>();
//    }
//
//    [MenuItem ("GameObject/Circle %&c")]
//    static void CreateCircle() {
//        Create<CircleProperty>();
//    }
//
//    [MenuItem ("GameObject/Rect %&r")]
//    static void CreateRect() {
//        Create<RectProperty>();
//    }
//
//    static void Create<T>() {
//        GameObject prefab;
//        if (typeof(T) == typeof(LineProperty)) {
//            prefab = Resources.Load<GameObject>(LINE_PREFAB_PATH);
//        } else if (typeof(T) == typeof(CircleProperty)) {
//            prefab = Resources.Load<GameObject>(CIRCLE_PREFAB_PATH);
//        } else if (typeof(T) == typeof(RectProperty)) {
//            prefab = Resources.Load<GameObject>(RECT_PREFAB_PATH);
//        } else {
//            Debug.LogError("Unrecognized class: " + typeof(T));
//            return;
//        }
//
//        var go = Instantiate(prefab, Camera.main.transform.position.SwapZ(0), Quaternion.identity) as GameObject;
//        var property = go.GetComponent<IObjectProperty>();
//        DefaultShapeStyle.Apply(property);
//        property.OnUpdate();
//        Selection.activeGameObject = go;
//    }
//
//    // we need to get previous element's thickness, regardless of circle etc.
//    // notification? or editor cached property
//    void OnGUI () {
//        if (GUILayout.Button("Line")) {
//            CreateLine();
//        }
//
//        if (GUILayout.Button("Circle")) {
//            CreateCircle();
//        }
//
//        if (GUILayout.Button("Rect")) {
//            CreateRect();
//        }
//    }
}
