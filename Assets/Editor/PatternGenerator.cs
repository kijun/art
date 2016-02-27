using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PatternGenerator : MonoBehaviour {

    const float cameraWidth = 5.63f;
    const float cameraHeight = 10f;
    const float gapBetweenPatterns = cameraWidth*2;
    const string patternDirPath = "Assets/Patterns";
    const string patternBackgroundPath = "Assets/Prefabs/PatternBackground.prefab";
    const string playerPrefabPath = "Assets/Prefabs/Player.prefab";
    // activated by monobehaviour

	// Add menu named "My Window" to the Window menu
	[MenuItem ("Patterns/Create New Pattern %&n")]
	public static void CreatePattern () {
        CancelImmediateActivation();
        // count the number of patterns in folder
        var dir = new DirectoryInfo(patternDirPath);
        FileInfo[] info = dir.GetFiles("*.prefab");
        foreach (FileInfo f in info) {
            Debug.Log(f);
        }

        int patNum = info.Length + 1;
        var go = new GameObject("Pattern"+ patNum);
        go.transform.position = new Vector3(CenterX(patNum), 0, 0);

        var background = InstantiateFromPath(patternBackgroundPath);
        Debug.Log(AssetDatabase.LoadAssetAtPath<GameObject>(patternBackgroundPath));
        Debug.Log(background);
        background.transform.localScale = new Vector2(cameraWidth, cameraHeight*2);
        background.transform.SetParent(go.transform);
        background.transform.localPosition = new Vector3(0, cameraHeight, 100); // clip

        /*
        var bounds = new Bounds(
                       new Vector2(cameraWidth/2, cameraHeight),
                       new Vector2(cameraWidth, cameraHeight*2));
        border.bounds = bounds;
        */

        /*
		// Get existing open window or if none, make a new one:
		DialogueSystemWindow window = (DialogueSystemWindow)EditorWindow.GetWindow(
                typeof (DialogueSystemWindow));
        window.Initialize();
		window.Show();
        */
        // if
        var transforms = Selection.transforms;
        if (transforms.Length > 0 && EditorUtility.DisplayDialog("Move " +transforms.Length+" object(s)?", "", "Move", "Cancel")) {
            float minY = float.PositiveInfinity;
            float minX = float.PositiveInfinity;
            float maxX = float.NegativeInfinity;
            foreach (var t in transforms) {
                t.SetParent(go.transform);
                var bounds = GetBoundsOfTransform(t);
                //t.GetComponent<Renderer>().bounds;
                minY = Mathf.Min(bounds.min.y, minY);
                minX = Mathf.Min(bounds.min.x, minX);
                maxX = Mathf.Max(bounds.max.x, maxX);
            }
            // we center X for now
            Debug.Log(minY + "/" + minX + "/" + maxX);
            float midX = (minX+maxX)/2f;

            foreach (var t in transforms) {
                t.localPosition = new Vector2(t.position.x-midX, t.position.y-minY);
            }
        };
	}

    static Bounds GetBoundsOfTransform(Transform t) {
        var renderer = t.GetComponent<Renderer>();
        var bounds = new Bounds(t.position, Vector3.zero);
        if (renderer != null) {
            bounds.Encapsulate(renderer.bounds);
        }
        foreach (Transform child in t) {
            renderer = child.GetComponent<Renderer>();
            if (renderer != null) {
                bounds.Encapsulate(renderer.bounds);
            }
        }
        return bounds;
    }

    [MenuItem ("Patterns/Play Pattern %&p")]
    public static void PlayPattern() {
        Debug.Log("AAA");
        CancelImmediateActivation();
        var pattern = Selection.activeGameObject;
        var startPos = pattern.transform.position - new Vector3(0, 1, 0);
        SetupPlayerAndCamera(startPos);
        EditorApplication.isPlaying = true;
    }

    [MenuItem ("Patterns/Activate Object %&o")]
    public static void ActivateObjects() {
        var pl = GameObject.FindObjectOfType<PatternLauncher>();
        pl.toActivateInPlayMode = Selection.gameObjects;
        EditorApplication.isPlaying = true;
    }

    public static void CancelImmediateActivation() {
        var pl = GameObject.FindObjectOfType<PatternLauncher>();
        pl.toActivateInPlayMode = new GameObject[0];
    }

    public static void SetupPlayerAndCamera(Vector3 startPos) {
        Camera.main.transform.position = new Vector3(startPos.x, cameraHeight/2+startPos.y, -10);
        var player = GameObject.FindWithTag("Player");
        player.transform.position = startPos.IncrY(0.5f);
    }

    public static float CenterX(int patNum) {
        return (cameraWidth + gapBetweenPatterns) * patNum;
    }

    public static GameObject InstantiateFromPath(string path) {
        return (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(path));
    }
}

public class PatternController : MonoBehaviour {

    void Start() {
    }
}
