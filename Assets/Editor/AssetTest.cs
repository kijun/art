//CreateCameraLocationAsset.cs
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;


public class AssetTest
{
    [MenuItem("Custom/Create asset")]
    public static void CreateMyAsset() {
        var d = new Dialogue(1);
        d.priority = 5;
        d.lines = "Hello || World";

        var dh = ScriptableObject.CreateInstance<DialogueHolder>();

        dh.dialogues.Add(d);

        AssetDatabase.CreateAsset(dh, "Assets/dialogue.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
    }

    [MenuItem("Custom/Load asset")]
    public static void LoadMyAsset() {
        var dh = AssetDatabase.LoadAssetAtPath<DialogueHolder>("Assets/dialogue.asset");

        Debug.Log(dh.dialogues[0]);
        Debug.Log(dh.dialogues[0].lines);
    }
}
