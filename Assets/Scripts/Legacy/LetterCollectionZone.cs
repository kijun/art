using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (BoxCollider2D))]
public class LetterCollectionZone : MonoBehaviour {

    public string wordToCollect = "wonder";
    public TextMesh banner;
    public float fadeTime = 2f;
    public Color32 colorOnPickup;
    public Transform cameraLockPosition;

    private List<char> collected = new List<char>();
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
            Lock();
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

    void Lock() {
        //player.LockCurrentRegion();
        Camera.main.GetComponent<CameraController>().LockCamera(cameraLockPosition.position);
    }

    void Unlock() {
        //player.UnlockCurrentRegion();
        Camera.main.GetComponent<CameraController>().UnlockCamera();
    }

    string BuildColoredString() {
        string text = "";
        var unusedChars = new List<char>(collected);

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
        Unlock();
    }
}
