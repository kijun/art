using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class DialogueSystemWindow: EditorWindow {
	string myString = "Hello World";
	bool groupEnabled;
	bool myBool = true;
	float myFloat = 1.23f;

    //private DialogueHolder dialogueHolder;
    private DialogueHolder dh;

	// Add menu named "My Window" to the Window menu
	[MenuItem ("Window/Dialogue System")]
	static void Init () {
        Debug.Log("init");
		// Get existing open window or if none, make a new one:
		DialogueSystemWindow window = (DialogueSystemWindow)EditorWindow.GetWindow(
                typeof (DialogueSystemWindow));
        window.Initialize();
		window.Show();
	}

    void Initialize() {
        Debug.Log("this init");
    }

    void Save() {
        Debug.LogError("SAVE");
        if (!AssetDatabase.Contains(dialogueHolder)) {
            AssetDatabase.CreateAsset(dialogueHolder, "Assets/dialogue.asset");
        }
        AssetDatabase.SaveAssets();
    }

    DialogueHolder dialogueHolder {
        get {
            Debug.Log("dialogue holder");
            if (dh == null) {
                Debug.Log("trying to load");
                dh = AssetDatabase.LoadAssetAtPath<DialogueHolder>("Assets/dialogue.asset");
                Debug.Log(dh);
                if (dh == null) {
                    Debug.Log("dh is null");
                    dh = ScriptableObject.CreateInstance<DialogueHolder>();
                }
            }
            return dh;
        }
    }


/*
 * 0. need two windows - Dialogue/DialogueSystem
 * 1. ds- load every dialogue, present them in a scrollview with an open button
 * 2. dw- textare for lines, + de
 * if there's none, create
 */

	void OnGUI () {
        List<Dialogue> ds = dialogueHolder.dialogues;
        Dialogue toRemove = null;

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+")) {
            var newDialogueID = 0;
            if (ds.Count > 0) {
                newDialogueID = ds[ds.Count-1].id + 1;
            }
            DialogueWindow.Open(new Dialogue(newDialogueID), this);
        }
        GUILayout.EndHorizontal();

        foreach (Dialogue d in dialogueHolder.dialogues) {
            GUILayout.BeginHorizontal();
            GUILayout.Label(d.id + ": " + d.lines.Substring(0, Mathf.Min(20, d.lines.Length)));
            if (GUILayout.Button("edit")) {
                DialogueWindow.Open(d, this);
            }
            if (GUILayout.Button("del")) {
                toRemove = d;
            }
            GUILayout.EndHorizontal();
        }
        if (toRemove != null) {
            dialogueHolder.dialogues.Remove(toRemove);
        }
	}

    void OnDestroy() {
        Save();
    }

    public void AddDialogue(Dialogue d) {
        if (!dialogueHolder.dialogues.Contains(d)) {
            dialogueHolder.dialogues.Add(d);
        }
        EditorUtility.SetDirty(dialogueHolder);
        Save();
        Repaint();
    }
}


public class DialogueWindow : EditorWindow {

    private Dialogue dialogue;

    private DialogueSystemWindow dsw;

    public static void Open(Dialogue d, DialogueSystemWindow dsw) {
		DialogueWindow window = (DialogueWindow)EditorWindow.GetWindow(
                typeof (DialogueWindow));
        window.Initialize(d, dsw);
		window.Show();
	}

    void Initialize(Dialogue d, DialogueSystemWindow dsw) {
        dialogue = d;
        this.dsw = dsw;
    }

	void OnGUI () {
        dialogue.id = EditorGUILayout.IntField("ID", dialogue.id);
        dialogue.priority = EditorGUILayout.IntField("Priority", dialogue.priority);
        var lines = "";
        if (dialogue.lines != null) {
            lines = dialogue.lines;
        }
        dialogue.lines = EditorGUILayout.TextArea(lines);
        if (GUILayout.Button("save")) {
            dsw.AddDialogue(dialogue);
            this.Close();
        }
	}
}
