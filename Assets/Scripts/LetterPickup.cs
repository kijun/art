using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D), typeof (TextMesh))]
public class LetterPickup : MonoBehaviour {
    public Color colorOnPickup = new Color32(116, 116, 116, 1);

    private BoxCollider2D letterCollider;
    private LetterCollectionZone zone;
    private TextMesh letterMesh;
    private bool active = true;

	// Use this for initialization
	void Start () {
        letterCollider = GetComponent<BoxCollider2D>();
        letterMesh = GetComponent<TextMesh>();
        zone = transform.parent.GetComponent<LetterCollectionZone>();
	}

	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter2D(Collider2D other) {
        if (active && other.gameObject.tag == "Player") {
            zone.LetterCollected(letterMesh.text[0]);
            active = false;
            letterMesh.color = colorOnPickup;
        }
    }
}
