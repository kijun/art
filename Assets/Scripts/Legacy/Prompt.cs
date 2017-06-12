using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Prompt : MonoBehaviour {

    public float timeBetweenLine = 2f;
    public TextAsset promptText;
    public bool finished = false;

    private Queue<string> paragraphs = new Queue<string>();
    private Text textArea;

	// Use this for initialization
	void Start () {

	}

    void Awake () {
        textArea = GetComponent<Text>();
        string pg = "";
        foreach (var line in promptText.text.Split('\n')) {
            if (line.Equals("")) {
                if (pg.Length > 0) {
                    paragraphs.Enqueue(pg);
                    pg = "";
                }
            } else {
                pg += line + "\n";
            }
        }
        //StartCoroutine(RunPrompt());
        DisplayNextParagraph();
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space") && paragraphs.Count > 0) {
            DisplayNextParagraph();
        }
	}

    void DisplayNextParagraph() {
        textArea.text = paragraphs.Dequeue() + "\n";
    }
}
