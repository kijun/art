using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnowFlake : MonoBehaviour {

    public Rigidbody2D center;
    public List<Rigidbody2D> arms;
    public Range ejectAt = new Range(1.5f, 3f);
    public float ejectionSpeed = 4;

    Rigidbody2D rg2d;
    bool ejected;

    void Awake () {
        rg2d = GetComponent<Rigidbody2D>();
    }

	// Use this for initialization
	void Start () {
	}

    public void FallDown(float aV, Vector2 v) {
        Debug.Log("falling");
        rg2d.angularVelocity = aV;
        rg2d.velocity = v;
        StartCoroutine(Detach(Random.Range(ejectAt.minimum, ejectAt.maximum)));
    }

    IEnumerator Detach(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        transform.DetachChildren();
        var currVel = rg2d.velocity;
        foreach (Rigidbody2D arm in arms) {
            var dir = arm.position - center.position;
            Debug.Log(dir);
            arm.velocity = ejectionSpeed*Vector3.Normalize(dir);
        }
        center.velocity = currVel;
        Destroy(this.gameObject);
    }
}
