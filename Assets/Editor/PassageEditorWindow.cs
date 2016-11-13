using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;


public class Passage : Cell {
}

public class Cell : MonoBehaviour {
    /* Animation clip contains all animations specific to itself
     * if there are child objects, they contain their own animation clips
     * on launch, these are compiled and gathered into a single animation that controlls
     * the passage */

    public AnimationClip clip;

    public void Play(float time=0f) {
        var anim = GetComponent<Animation>();
        if (anim == null) {
            anim = gameObject.AddComponent<Animation>();
        }

        BuildAnimationClip();
        anim.AddClip(clip, "Default");
        anim.Play("Default");
        anim["Default"].time = time;
    }

    void BuildAnimationClip() {
        clip = new AnimationClip();

        // traverse
        foreach (var child in TraverseChildCells()) {
            var childPath = AnimationUtility.CalculateTransformPath(child.transform, transform);

            foreach (var curve in AnimationUtility.GetAllCurves(child.clip)) {
                clip.SetCurve(
                        System.IO.Path.Combine(childPath, curve.path),
                        curve.type,
                        curve.propertyName,
                        curve.curve
                );
            }
        }
    }

    public void SetKeyFrame(Type componentType, string propertyName, float time, float value) {
        var curve = AnimationUtility.GetEditorCurve(clip, null, componentType, propertyName);
        if (curve == null) {
            curve = new AnimationCurve();
        }
        // replace value or add key
        // dedup if no change
        bool replaced = false;
        for (int i = 0; i < curve.keys.Length; i++) {
            var key = curve.keys[i];
            if (key.time == time) {
                key.value = value;
                replaced = true;
                break;
            }
        }

        if (!replaced) {
            curve.AddKey(time, value);
        }
    }

    // localposition?
    // it should always be local - the problem is when
    public void SetPositionAtTime(Vector3 position, float time, GameObject target=null) {
        // if child is specified, this is a helper method that sets it
        if (target == null) {
            SetKeyFrame(typeof(Transform), "localPosition.x", time, position.x);
            SetKeyFrame(typeof(Transform), "localPosition.y", time, position.y);
            SetKeyFrame(typeof(Transform), "localPosition.z", time, position.z);
        }
        // for each child, call target
        // if target is inside current object, then set it
        // otherwise call children?
        // usually we'll just set it to
    }

    public void RemoveKeyFrame(Type componentType, string propertyName, float time) {
        var curve = AnimationUtility.GetEditorCurve(clip, null, componentType, propertyName);
        for (int i = 0; i < curve.keys.Length; i++) {
            if (curve.keys[i].time == time) {
                curve.RemoveKey(i);
                break;
            }
        }
    }

    public void MoveKeyFrame(Type componentType, string propertyName, float time, float timeto) {
        // TODO
    }

    protected IEnumerable<Cell> TraverseChildCells() {
        yield return this;
        foreach (Transform child in transform) {
            var childCell = child.gameObject.GetComponent<Cell>();
            if (childCell != null) yield return childCell;
        }
    }
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

/*
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
*/

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

        /*
        if (this.passage != null) {
            this.passage = passage;
        }
        */
    }


    void OnGUI() {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField(passage.gameObject.name);
        if (GUILayout.Button("Play")) {
            passage.Play(0);
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

