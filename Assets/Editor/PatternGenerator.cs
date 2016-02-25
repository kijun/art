using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class PatternGenerator {

    const float cameraWidth = 5.63f;
    const float cameraHeight = 10f;
    const float gapBetweenPatterns = cameraWidth*2;
    const string patternDirPath = "Assets/Patterns";
    const string patternBackgroundPath = "Assets/Prefabs/PatternBackground.prefab";

	// Add menu named "My Window" to the Window menu
	[MenuItem ("GameObject/Create New Pattern %&p")]
	public static void CreatePattern () {
        // count the number of patterns in folder
        var dir = new DirectoryInfo(patternDirPath);
        FileInfo[] info = dir.GetFiles("*.prefab");
        foreach (FileInfo f in info) {
            Debug.Log(f);
        }

        int patNum = info.Length + 1;
        var go = new GameObject("Pattern"+ patNum);
        float goXCenter = (cameraWidth + gapBetweenPatterns) * patNum;
        go.transform.position = new Vector3(goXCenter, 0, 0);

        var background = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(patternBackgroundPath));
        Debug.Log(AssetDatabase.LoadAssetAtPath<GameObject>(patternBackgroundPath));
        Debug.Log(background);
        background.transform.localScale = new Vector2(cameraWidth, cameraHeight*2);
        background.transform.SetParent(go.transform);
        background.transform.localPosition = new Vector2(0, cameraHeight);

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
	}
}

public class PatternController : MonoBehaviour {
}
