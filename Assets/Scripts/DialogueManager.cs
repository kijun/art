using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState {

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

public class Dialogue {
    public int priority = 0;
    public int id {
        get;
        private set;
    }
    private List<Condition> conditions = new List<Condition>();
    public string lines;


    public Dialogue(int id) {
        this.id = id;
    }

    public bool Ready(GameState states, Dictionary<int, Dialogue> completed) {
        foreach (var cond in conditions) {
            if (!cond.Check(states, completed)) return false;
        }

        return true;
    }
}

public abstract class Condition {
    public abstract bool Check(GameState states, Dictionary<int, Dialogue> completed);
}

public class StateCondition : Condition {
    public override bool Check(GameState states, Dictionary<int, Dialogue> completed) {
        // TODO: how?
        return false;
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

