using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DialogueHolder : ScriptableObject {
    public List<Dialogue> dialogues = new List<Dialogue>();

    /*public DialogueHolder (Dialogue d) {
        dialogue = d;
    }*/
}

