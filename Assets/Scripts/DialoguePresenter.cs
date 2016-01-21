using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialoguePresenter : MonoBehaviour {
    public DialogueHolder dialogueHolder;
    public float timeBetweenLines = 3f;

    private DialogueManager dm;
    private Text textLabel;

	// Use this for initialization
	void Awake () {
        dm = new DialogueManager(dialogueHolder.dialogues);
        textLabel = GetComponent<Text>();
        StartCoroutine(PresentDialogue());
	}

    IEnumerator PresentDialogue() {
        while (dm.HasDialogue()) {
            var d = dm.NextDialogue();
            if (d != null) {
                foreach (var line in d.lines.Split('\n')) {
                    textLabel.text = line;
                    // TODO play sound
                    yield return new WaitForSeconds(timeBetweenLines);
                }
                dm.DialogueCompleted(d);
            }
        }
    }
}
