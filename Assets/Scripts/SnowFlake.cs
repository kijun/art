using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnowFlake : MonoBehaviour {

    public GameObject center;
    public List<GameObject> arms;
    public Range ejectAt = new Range(1.5f, 3f);
    public float ejectionSpeed = 4;

    Rigidbody2D rg2d;
    bool ejected;

    void Awake () {
        rg2d = GetComponent<Rigidbody2D>();
    }

	// Use this for initialization
	void Start () {
        //FallDown(50, new Vector2(-2, -4));
	}

    public void FallDown(float aV, Vector2 v) {
        rg2d.angularVelocity = aV;
        rg2d.velocity = v;
        StartCoroutine(Detach(Random.Range(ejectAt.minimum, ejectAt.maximum)));
    }

    IEnumerator Detach(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        transform.DetachChildren();
        var currVel = rg2d.velocity;
        // eject arms
        foreach (GameObject armGO in arms) {
            Rigidbody2D armRB = armGO.AddComponent<Rigidbody2D>();
            armRB.gravityScale = 0;

            var dir = armRB.position - (Vector2)center.transform.position;
            armRB.velocity = ejectionSpeed*Vector3.Normalize(dir);
        }
        // eject center
        /*
        Rigidbody2D centerRB = center.AddComponent<Rigidbody2D>();
        centerRB.gravityScale = 0;
        centerRB.velocity = currVel;
        Destroy(centerRB);
        */
        Destroy(center);
        Destroy(this.gameObject);
    }
}
