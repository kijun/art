using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


public class PassageDataWrapper {
    public PassageData passageData;
    // something something prefab
}

// every passage has a passage data -
public class PassageData {
    public CellData[] cells;
}

// each cell corresponds to a GameObject
public class CellData {
    // public reference to music
    // public class;
    public CellData[] children;
    public TransformKeyFrame[] keyframes;
    // so while the object is in real life, this should point to the actual gameobject
    // what if the object is deleted from the scene?
    // then this would become nothing
    // is there a way to find the object from prefab?
    // i think whenever passage data is changed (or passaage changes)
    // we need to autosave the prefab (or have a button to commit it)
    // then celldata would refer to the gameobject within the prefab
    // and during instantiation we might be able to link it up.
    // so save two prefa - nah.
    // hmm. or yea-
    public GameObject go;
    // perhaps a linked list
}

public class TransformKeyFrame {
    public float time;
    // all local
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}

public class KeyframePlayer : MonoBehaviour {
    public TransformKeyFrame[] keyframe;

    public void Update() {
        // interpolate position, rotation, scale
    }
}

public class Passage : MonoBehaviour {
    public CellData[] cells;

    public void Awake() {
        foreach (var c in cells) {
            // when we retry, we reinstantiate prefab
            // so we might not even need this
        }
    }

    public void MoveToTime(float time) {
        foreach (var c in cells) {
            c.MoveToTime(time);
        }
    }

    public void Play() {
    }
}

public class Cell : MonoBehaviour {
    public KeyFrame[] keyFrames;

    void Update() {
    }

    public void MoveToTime(float time) {
        foreach (var c in cells) {
            c.MoveToTime(time);
        }
    }

    void Interpolate(float time) {
        // hmm
    }
}

/*
 * How does each keyframe get rendered?
 *
 * Script keeps a list of items?
 * Or a script gets attached to each item - and knows how to move itself around?
 *
 * How do I attach keyframe to a static object?
 * Is there a way to serialize it while remembering it in prefab configuration?
 * Passage data corresponds to prefab. When it is materialized, attach it to a figure?
 */

public class PassageEditorWindow : EditorWindow {
    Passage passage;

	[MenuItem ("Window/Passage Editor")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		PassageEditorWindow window = (PassageEditorWindow)EditorWindow.GetWindow(
                typeof (PassageEditorWindow));
        window.Initialize();
		window.Show();
        // if passage was selected
        var go = Selection.activeGameObject;
        var passage = go.GetComponent<Passage>();

        if (this.passage != null) {
            this.passage = passage;
        }
	}


    void OnGUI() {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField(currentPassage.gameObject.name);
        if (GUILayout.Button("Play")) {
            passage.Play(0);
        }

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

        // Draw Top Bar
        // - Write Passage Name
        // - Play btn

        // Draw Timeline
        // - On click, move forward/backward in time

        // Draw each cell
        // - Left column
        // -- Drawer btn if child exists
        // -- Name
        // -- Play btn
        // -- + btn
        // - Right Column (Timeline)
        // -- Background On click
        // --- if keyframe, step into time
        // --- if nokeyframe, make a keyframe. further edits move them
        // - Rectangle
        // -- On Click, expand?
        // -- On Drag, move
        // -- On click (right edge, repeat)

        // When blocks are added, add them to static pile
        // if they are static
    }
    // deselect, move to origin
    //
    void Save() {

    }


    void Initialize() { }

    void Save() {
        Debug.LogError("SAVE");
        /*
        if (!AssetDatabase.Contains(dialogueHolder)) {
            AssetDatabase.CreateAsset(dialogueHolder, "Assets/dialogue.asset");
        }
        */
        AssetDatabase.SaveAssets();
    }

    /*
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
    */


/*
 */

	void OnGUI () {
        /*
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
        */
	}

    void OnDestroy() {
        Save();
    }

    /*
    public void AddDialogue(Dialogue d) {
        if (!dialogueHolder.dialogues.Contains(d)) {
            dialogueHolder.dialogues.Add(d);
        }
        EditorUtility.SetDirty(dialogueHolder);
        Save();
        Repaint();
    }
    */
}


/*
public class DialogueWindow : EditorWindow {

    private Dialogue dialogue;

    private PassageEditorWindow dsw;

    public static void Open(Dialogue d, PassageEditorWindow dsw) {
		DialogueWindow window = (DialogueWindow)EditorWindow.GetWindow(
                typeof (DialogueWindow));
        window.Initialize(d, dsw);
		window.Show();
	}

    void Initialize(Dialogue d, PassageEditorWindow dsw) {
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
*/

