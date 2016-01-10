using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {
    public static UIManager instance;

    public float timeBetweenLine = 1f;

    private Queue<string> lines = new Queue<string>();
    private bool running;
    private Text textRenderer;

    public void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        textRenderer = GetComponentInChildren<Text>();
    }

    public void ShowText(string text) {
        foreach (var line in text.Split('\n')) {
            lines.Enqueue(line);
        }
        if (!running) {
            StartCoroutine(DisplayLine());
        }
    }

    IEnumerator DisplayLine() {
        if (running) yield break;

        running = true;

        while (lines.Count > 0) {
            var t = lines.Dequeue();
            textRenderer.text = t;
            yield return new WaitForSeconds(timeBetweenLine);
        }

        textRenderer.text = null;

        running = false;
    }
}
