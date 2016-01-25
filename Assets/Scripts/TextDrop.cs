using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextDrop : MonoBehaviour {

    public TextMesh letterPrefab;
    public float pauseBeforeDrop = 3f;
    public float pauseAfterDrop = 3f;
    public float pauseBetweenDrops = 1f;
    public float fadeInDuration = 3f;
    public float fadeOutDuration = 3f;
    public List<string> words;
    public float letterSpacing = 0.1f;
    public float gravityScale = 1.5f;

    private List<TextMesh> letters = new List<TextMesh>();

	// Use this for initialization
	void Start () {
        //GetComponent<Renderer>().sortingLayerName = "Midground";

        StartCoroutine(DropWords());
	}

    IEnumerator DropWords() {
        while (true) {
            // reset words
            letters.Clear();

            var word = words[Random.Range(0, words.Count)];
            float midpoint = word.Length/2.0f;
            for (int i = 0; i<word.Length; i++) {
                string letter = word.Substring(i, 1);
                TextMesh lm = CreateLetterMesh(letter);
                letters.Add(lm);

                // position letter
                lm.transform.parent = this.transform;
                lm.transform.localPosition = new Vector3((i - midpoint) * letterSpacing, 0, -2);
            }
            yield return StartCoroutine(FadeLetters(0, 1));

            yield return new WaitForSeconds(pauseBeforeDrop);

            var letterToDrop = letters[Random.Range(0, letters.Count)];
            letterToDrop.GetComponent<Rigidbody2D>().gravityScale = gravityScale;

            yield return new WaitForSeconds(pauseAfterDrop);

            yield return StartCoroutine(FadeLetters(1, 0));

            DestroyLetters();

            yield return new WaitForSeconds(pauseBetweenDrops);
        }
    }

    IEnumerator FadeLetters(float from, float to) {
        var fadeOutStart = Time.time;

        while (fadeOutStart + fadeOutDuration> Time.time) {
            var alpha = Mathf.Lerp(from, to, (Time.time - fadeOutStart) / fadeOutDuration);
            foreach (var l in letters) {
                Renderer r = l.GetComponent<Renderer>();
                var newcolor = r.material.color;
                newcolor.a = alpha;
                r.material.color = newcolor;
            }
            yield return null;
        }
    }

    void DestroyLetters() {
        foreach (var l in letters) {
            Destroy(l.gameObject);
        }
    }

    TextMesh CreateLetterMesh(string letter) {
        var letterMesh = Instantiate<TextMesh>(letterPrefab);
        letterMesh.text = letter;
        return letterMesh;
    }

}
