using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D), typeof (Rigidbody2D))]
public class ShadowShipController : MonoBehaviour {

    public float speed = 1.5f;
    public float fadeTime = 2f;

    private Rigidbody2D rgbd;
    private bool destroying = false;

	// Use this for initialization
	void Start () {
        // hack opacity
        var material = GetComponent<Renderer>().material;
        var c = material.color;
        c.a = 0.5f;
        material.color = c;

        rgbd = GetComponent<Rigidbody2D>();
        rgbd.velocity = new Vector2(0, speed);
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("Bullet") && !destroying) {
            destroying = true;
            StartCoroutine(DestroyShip());
        }
    }

    IEnumerator DestroyShip() {
        rgbd.velocity = Vector2.zero;
        var material = GetComponent<Renderer>().material;
        var startColor = material.color;
        var endColor = startColor; // struct
        endColor.a = 0;
        var startTime = Time.time;
        while (Time.time < startTime + fadeTime) {
            material.color = Color.Lerp(startColor, endColor, (Time.time - startTime)/fadeTime);
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
