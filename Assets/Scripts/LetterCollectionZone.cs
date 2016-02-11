using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (BoxCollider2D))]
public class LetterCollectionZone : MonoBehaviour {

    public string wordToCollect = "wonder";
    public TextMesh banner;
    // right now can't have same letters
    public List<char> collected = new List<char>();
    //public HashSet<char> collected = new HashSet<char>();
    public float fadeTime = 2f;
    public Color32 colorOnPickup;

    private BoxCollider2D fixCameraZone;
    private PlayerController player;
    private bool solved = false;
	// Use this for initialization
	void Start () {
        fixCameraZone = GetComponent<BoxCollider2D>();
        banner.text = BuildColoredString();
	}

	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter2D(Collider2D other) {
        if (solved) return;
        if (other.gameObject.tag == "Player") {
            player = other.GetComponent<PlayerController>();
//            baseSpeedCache = player.yBaseSpeed;
            player.LockCurrentRegion();
        }
    }

    public void LetterCollected(char letter) {
        collected.Add(letter);
        banner.text = BuildColoredString();
        if (collected.Count == wordToCollect.Length) {
            solved = true;
            StartCoroutine(ToNextPuzzle());
        }
    }

    string BuildColoredString() {
        string text = "";
        List<char> unusedChars = new List<char>(collected);

        foreach (char l in wordToCollect) {
            if (unusedChars.Contains(l)) {
                unusedChars.Remove(l);

                string coloredLetter = string.Format("<color=#{0}{1}{2}{3}>{4}</color>",
                        // todo extension
                                                     colorOnPickup.r.ToString("X2"),
                                                     colorOnPickup.g.ToString("X2"),
                                                     colorOnPickup.b.ToString("X2"),
                                                     colorOnPickup.a.ToString("X2"),
                                                     l);
                Debug.Log(coloredLetter.Substring(1));

                text += coloredLetter;
            } else {
                text += l;
            }
        }
        return text;
    }

    IEnumerator ToNextPuzzle() {
        foreach (Transform childT in transform) {
            var fader = childT.gameObject.AddComponent<ObjectFader>();
            fader.FadeAndDestroy(fadeTime);
        }
        yield return new WaitForSeconds(fadeTime);
        player.UnlockCurrentRegion();
    }
}
