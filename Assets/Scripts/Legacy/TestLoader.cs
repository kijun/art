using UnityEngine;
using System.Collections;

public class TestLoader : MonoBehaviour {
    public Dialogue dialogue;
    public DialogueHolder dialogueHolder;

	// Use this for initialization
	void Start () {

	}

    void Awake() {
        Debug.Log("Dialouggugugugug" + dialogueHolder.dialogues[0].lines);
    }

	// Update is called once per frame
	void Update () {

	}
}
