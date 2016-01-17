using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Prompt : MonoBehaviour {

    public float timeBetweenLine = 2f;
    public TextAsset promptText;
    public bool finished = false;

    private Text textArea;

	// Use this for initialization
	void Start () {

	}

    void Awake () {
        textArea = GetComponent<Text>();
        StartCoroutine(RunPrompt());
    }

    IEnumerator RunPrompt() {
        foreach (var line in promptText.text.Split('\n')) {
            textArea.text += "\n" + line;
            // TODO play sound
            yield return new WaitForSeconds(timeBetweenLine);
        }
        finished = true;
    }

	// Update is called once per frame
	void Update () {

	}
}
