using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SegmentGenerator : MonoBehaviour {

    /***** CONST *****/
    /*
    const float cameraWidth = 5.63f;
    const float cameraHeight = 10f;
    const float gapBetweenSegments = cameraWidth*2;
    */
    const float DEFAULT_BACKGROUND_WIDTH = 6f; // set to camera ratio
    const float DEFAULT_BACKGROUND_HEIGHT = 10f;
    const float GAP_BETWEEN_SEGMENTS = DEFAULT_BACKGROUND_WIDTH * 2;

    const string SEGMENT_DIR_PATH = "Assets/Segments";
    const string SEGMENT_BACKGROUND_PATH = "Assets/Prefabs/SegmentBackground.prefab";
    const string PLAYER_PREFAB_PATH = "Assets/Prefabs/Player.prefab";
    const string LEVEL_DIR_PATH = "Assets/Levels";
    // activated by monobehaviour


    /***** PUBLIC: STATIC MENU ITEMS *****/
	// Add menu named "My Window" to the Window menu
	[MenuItem ("Segments/Create New Segment %&n")]
	public static void CreateSegment () {
        // Instantiate object with following segment name
        int patNum = CountFilesInPathWithExt(SEGMENT_DIR_PATH, "prefab");
        var segment = (new GameObject("Segment" + patNum)).AddComponent<Segment>();
        segment.transform.position = new Vector3(CenterX(patNum), 0, 0);

        var background = InstantiateFromPath(SEGMENT_BACKGROUND_PATH);
        background.transform.localScale = new Vector2(DEFAULT_BACKGROUND_WIDTH,
                DEFAULT_BACKGROUND_HEIGHT);
        background.transform.SetParent(segment.transform);
        background.transform.localPosition = new Vector3(0, DEFAULT_BACKGROUND_HEIGHT/2f, 100); // clip

        // if transforms are selected, give option to set them as
        // children of the new segment
        var transforms = Selection.transforms;
        if (transforms.Length > 0 && EditorUtility.DisplayDialog("Move " +transforms.Length+" object(s)?", "", "Move", "Cancel")) {
            float minY = float.PositiveInfinity;
            float minX = float.PositiveInfinity;
            float maxX = float.NegativeInfinity;
            foreach (var t in transforms) {
                t.SetParent(segment.transform);
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

    [MenuItem ("Segments/Play Segment %&p")]
    public static void PlaySegment() {
        CancelImmediateActivation();
        var segment = Selection.activeGameObject.GetComponent<Segment>();
        EditorApplication.isPlaying = true;
        Debug.Log(segment);
        segment.PlaySegment(
                GameObject.FindObjectOfType<Player>(),
                GameObject.FindObjectOfType<CameraController>(),
                true);
    }

    [MenuItem ("Segments/Create Level Asset")]
    public static void CreateLevelAsset() {
        int nextLevel = CountFilesInPathWithExt(LEVEL_DIR_PATH, "asset") + 1;
        LevelSegments lh = ScriptableObject.CreateInstance<LevelSegments>();
        AssetDatabase.CreateAsset(lh, LEVEL_DIR_PATH + "/Level"+nextLevel+".asset");
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Assets/Load Level")]
    [MenuItem ("Segments/Load Level")]
    public static void LoadLevel() {
        var ld = Selection.activeObject as LevelSegments;
        var go = new GameObject(ld.name);

        if (ld != null) {
            float nextY = 0;
            foreach (var segment in ld.segments) {
                var newPat = Instantiate(segment,
                        new Vector3(0, nextY, 0),
                        Quaternion.identity) as GameObject;
                foreach (Transform child in newPat.transform) {
                    if (child.tag == "SegmentBounds") {
                        nextY = child.GetComponent<Renderer>().bounds.max.y;
                    }
                }

                newPat.transform.SetParent(go.transform);
            }
        }
    }

    /*
    [MenuItem ("Segments/Activate Object %&o")]
    public static void ActivateObjects() {
        var pl = GameObject.FindObjectOfType<SegmentLauncher>();
        pl.toActivateInPlayMode = Selection.gameObjects;
        EditorApplication.isPlaying = true;
    }

    [MenuItem ("Segments/Create Level Asset")]
    public static void CreateLevelAsset() {
        int nextLevel = CountFilesInPathWithExt(LEVEL_DIR_PATH, "asset") + 1;
        LevelHolder lh = ScriptableObject.CreateInstance<LevelHolder>();
        AssetDatabase.CreateAsset(lh, LEVEL_DIR_PATH + "/Level"+nextLevel+".asset");
        AssetDatabase.SaveAssets();
    }

    [MenuItem ("Segments/Load Level")]
    public static void LoadLevel() {
        var ld = Selection.activeObject as LevelHolder;
        var go = new GameObject(ld.name);

        if (ld != null) {
            float nextY = 0;
            foreach (var segment in ld.segments) {
                var newPat = Instantiate(segment,
                        new Vector3(0, nextY, 0),
                        Quaternion.identity) as GameObject;
                foreach (Transform child in newPat.transform) {
                    if (child.tag == "SegmentBounds") {
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
//    }


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
//        var pl = GameObject.FindObjectOfType<SegmentLauncher>();
//        pl.toActivateInPlayMode = new GameObject[0];
    }

    static void SetupPlayerAndCamera(Vector3 startPos) {
        Camera.main.transform.position = new Vector3(startPos.x,
                DEFAULT_BACKGROUND_HEIGHT/2+startPos.y, -10);
        var player = GameObject.FindWithTag("Player");
        player.transform.position = startPos.IncrY(0.5f);
        // segment
    }

    static float CenterX(int patNum) {
        return (DEFAULT_BACKGROUND_WIDTH + GAP_BETWEEN_SEGMENTS) * patNum;
    }

    static GameObject InstantiateFromPath(string path) {
        return (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(path));
    }
}
