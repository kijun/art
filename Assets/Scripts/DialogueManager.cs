using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState {

    public bool running;
    public float altitude;
    public int destroyedComets;
    public float gameTime;

    private static GameState inst;

    private GameState() {
    }

    public static GameState Instance {
        get {
            if (inst == null) {
                inst = new GameState();
            }

            return inst;
        }
    }
}

public class DialogueManager {
    private List<Dialogue> dialogues = new List<Dialogue>();
    private SortedDictionary<int, Dialogue> remainingDialogues = new SortedDictionary<int, Dialogue>();
    private Dictionary<int, Dialogue> completedDialogues = new Dictionary<int, Dialogue>();
    //private Dialogue currentDialogue;

    public DialogueManager(List<Dialogue> dg) {
        dialogues = dg;
        foreach (var d in dg) {
            remainingDialogues.Add(d.id, d);
        }
    }

    public Dialogue NextDialogue() {
        Dialogue nextDialogue = null;
        foreach (var d in remainingDialogues.Values) {

            if (nextDialogue == null) {
                if (d.Ready(GameState.Instance, completedDialogues)) {
                    nextDialogue = d;
                }
            }

            else if (d.priority < nextDialogue.priority &&
                     d.Ready(GameState.Instance, completedDialogues)) {
                nextDialogue = d;
            }
        }
        return nextDialogue;
    }

    public bool HasDialogue() {
        return remainingDialogues.Count > 0;
    }

    public void DialogueCompleted(Dialogue d) {
        remainingDialogues.Remove(d.id);
        completedDialogues.Add(d.id, d);
    }

    public void LoadDialogues() {
    }

    public void SaveDialogues() {
    }

    public void SaveDialogue() {
    }
}

public class DialogueHolder : ScriptableObject {
    public List<Dialogue> dialogues = new List<Dialogue>();

    /*public DialogueHolder (Dialogue d) {
        dialogue = d;
    }*/
}


[System.Serializable]
public class Dialogue {
    public int priority = 0;
    public int id {
        get;
        set;
    }
    private List<Condition> conditions;

    [TextArea(3,10)]
    public string lines;


    public Dialogue(int id) {
        this.id = id;
        conditions = new List<Condition>();
    }

    public bool Ready(GameState states, Dictionary<int, Dialogue> completed) {
        if (conditions != null) {
            foreach (var cond in conditions) {
                if (!cond.Check(states, completed)) return false;
            }
        }

        return true;
    }
}

public abstract class Condition {
    public abstract bool Check(GameState states, Dictionary<int, Dialogue> completed);
}

public class AltitudeStateCondition : Condition {
    public float value;

    public override bool Check(GameState states, Dictionary<int, Dialogue> completed) {
        // TODO: how?
        return value > states.altitude;
    }
}

public class TimeStateCondition : Condition {
    public float value;

    public override bool Check(GameState states, Dictionary<int, Dialogue> completed) {
        // TODO: how?
        return value > states.gameTime;
    }
}

public class CometStateCondition : Condition {
    public float value;

    public override bool Check(GameState states, Dictionary<int, Dialogue> completed) {
        // TODO: how?
        return value > states.destroyedComets;
    }
}

public class DependencyCondition : Condition {
    public int dependentDialogueID;

    public override bool Check(GameState states, Dictionary<int, Dialogue> completed) {
        return completed.ContainsKey(dependentDialogueID);
    }
}



/*
 *
A dialogue can consist of multiple lines, in which case they are delivered sequentially.

UI for it - list of dialogues (with unique id) double click to edit dialogue or create - save button, add dependency

Depending on the type you have a dropdown or a game condition checker
*/

