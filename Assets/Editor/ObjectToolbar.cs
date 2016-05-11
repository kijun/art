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

    void OnGUI () {
        if (GUILayout.Button("Line")) {
            var linePrefab = Resources.Load<GameObject>("Shapes/UnitLine");
            var line = Instantiate(linePrefab, Camera.main.transform.position.SwapZ(0), Quaternion.identity) as GameObject;
            Debug.Log(line);
            line.GetComponent<LineProperty>().OnPropertyChange();
        }

        if (GUILayout.Button("Circle")) {
        }

        if (GUILayout.Button("Triangle")) {
        }

        if (GUILayout.Button("Box")) {
        }
    }

    void OnSceneGUI() {
        if (!instantiating) return;
    }
}
