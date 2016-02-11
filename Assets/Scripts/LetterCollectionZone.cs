using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (BoxCollider2D))]
public class LetterCollectionZone : MonoBehaviour {

    public string wordToCollect = "wonder";
    public TextMesh banner;
    // right now can't have same letters
    public HashSet<char> collected = new HashSet<char>();
    public float fadeTime = 2f;

    private BoxCollider2D fixCameraZone;
    private PlayerController player;
    private bool solved = false;
    private float baseSpeedCache;
	// Use this for initialization
	void Start () {
        fixCameraZone = GetComponent<BoxCollider2D>();
        banner.text = wordToCollect;
	}

	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter2D(Collider2D other) {
        if (solved) return;
        if (other.gameObject.tag == "Player") {
            player = other.GetComponent<PlayerController>();
            baseSpeedCache = player.yBaseSpeed;
            player.yBaseSpeed = 0;
        }
    }

    public void LetterCollected(char letter) {
        collected.Add(letter);
        if (collected.Count == wordToCollect.Length) {
            solved = true;
            StartCoroutine(ToNextPuzzle());
        }
    }

    IEnumerator ToNextPuzzle() {
        foreach (Transform childT in transform) {
            var fader = childT.gameObject.AddComponent<ObjectFader>();
            fader.FadeAndDestroy(fadeTime);
        }
        yield return new WaitForSeconds(fadeTime);
        player.yBaseSpeed = baseSpeedCache;
    }
}
