using UnityEngine;
using UnityEditor;
using System.Collections;

public class ObjectToolbar : EditorWindow {

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
            Debug.Log("line");
        }

        if (GUILayout.Button("Circle")) {
        }

        if (GUILayout.Button("Triangle")) {
        }

        if (GUILayout.Button("Box")) {
        }
    }
}
