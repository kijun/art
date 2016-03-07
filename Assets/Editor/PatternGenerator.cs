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
    const string levelDirPath = "Assets/Levels";
    // activated by monobehaviour

	// Add menu named "My Window" to the Window menu
	[MenuItem ("Patterns/Create New Pattern %&n")]
	public static void CreatePattern () {
        CancelImmediateActivation();
        // count the number of patterns in folder
        int patNum = CountFilesInPathWithExt(patternDirPath, "prefab")+1;
        var go = new GameObject("Pattern"+ patNum);
        go.AddComponent<ZoneController>();
        go.transform.position = new Vector3(CenterX(patNum), 0, 0);

        var background = InstantiateFromPath(patternBackgroundPath);
        Debug.Log(AssetDatabase.LoadAssetAtPath<GameObject>(patternBackgroundPath));
        Debug.Log(background);
        background.transform.localScale = new Vector2(cameraWidth, cameraHeight*2);
        background.transform.SetParent(go.transform);
        background.transform.localPosition = new Vector3(0, cameraHeight, 100); // clip

        // if transforms are selected, give option to set them as
        // children of the new pattern
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

    [MenuItem ("Patterns/Create Level Asset")]
    public static void CreateLevelAsset() {
        int nextLevel = CountFilesInPathWithExt(levelDirPath, "asset") + 1;
        LevelHolder lh = ScriptableObject.CreateInstance<LevelHolder>();
        AssetDatabase.CreateAsset(lh, levelDirPath + "/Level"+nextLevel+".asset");
        AssetDatabase.SaveAssets();
    }

    [MenuItem ("Patterns/Load Level")]
    public static void LoadLevel() {
        var ld = Selection.activeObject as LevelHolder;
        var go = new GameObject(ld.name);

        if (ld != null) {
            float nextY = 0;
            foreach (var pattern in ld.patterns) {
                var newPat = Instantiate(pattern,
                        new Vector3(0, nextY, 0),
                        Quaternion.identity) as GameObject;
                foreach (Transform child in newPat.transform) {
                    if (child.tag == "PatternBounds") {
                        nextY = child.GetComponent<Renderer>().bounds.max.y;
                    }
                }

                newPat.transform.SetParent(go.transform);
            }
        }

        /*
        if (Selection.activeObject is LevelHolder) {
        }
        */
    }


    static int CountFilesInPathWithExt(string path, string extension) {
        var dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles("*."+extension);
        return info.Length;
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


    static void CancelImmediateActivation() {
        var pl = GameObject.FindObjectOfType<PatternLauncher>();
        pl.toActivateInPlayMode = new GameObject[0];
    }

    static void SetupPlayerAndCamera(Vector3 startPos) {
        Camera.main.transform.position = new Vector3(startPos.x, cameraHeight/2+startPos.y, -10);
        var player = GameObject.FindWithTag("Player");
        player.transform.position = startPos.IncrY(0.5f);
    }

    static float CenterX(int patNum) {
        return (cameraWidth + gapBetweenPatterns) * patNum;
    }

    static GameObject InstantiateFromPath(string path) {
        return (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(path));
    }
}
