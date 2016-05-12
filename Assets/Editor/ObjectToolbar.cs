using UnityEngine;
using UnityEditor;
using System.Collections;

public class ObjectToolbar : EditorWindow {

    bool instantiating = false;

	[MenuItem ("Window/Object Toolbar")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		ObjectToolbar window = (ObjectToolbar)EditorWindow.GetWindow(
                typeof (ObjectToolbar));
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

    // we need to get previous element's thickness, regardless of circle etc.
    // notification? or editor cached property
    void OnGUI () {
        if (GUILayout.Button("Line")) {
            var linePrefab = Resources.Load<GameObject>("Shapes/UnitLine");
            var line = Instantiate(linePrefab, Camera.main.transform.position.SwapZ(0), Quaternion.identity) as GameObject;
            var lineProperty = line.GetComponent<LineProperty>() as LineProperty;
            DefaultShapeStyle.ApplyToLine(lineProperty);
            line.GetComponent<LineProperty>().OnPropertyChange();
            // for line width and color, used previously changed value for consistency
        }

        if (GUILayout.Button("Circle")) {
        }

        if (GUILayout.Button("Triangle")) {
        }

        if (GUILayout.Button("Box")) {
        }
    }
}
