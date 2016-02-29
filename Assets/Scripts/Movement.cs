using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
public class Movement : MonoBehaviour {

    public Vector2 velocity;
    public float angularVelocity;
    public Transform topLimit;
    public Transform bottomLimit;
    public Transform anchor;

	// Use this for initialization
	void Start () {
        var rg = GetComponent<Rigidbody2D>();
        rg.velocity = velocity;
        rg.angularVelocity = angularVelocity;
	}

    void Update() {
        if (topLimit != null && bottomLimit != null)  {
            if (anchor.transform.position.y > topLimit.position.y)  {
                var rg = GetComponent<Rigidbody2D>();
                rg.velocity = new Vector2(velocity.x, -Mathf.Abs(velocity.y));
                Debug.Log("down");
            } else if (anchor.transform.position.y < bottomLimit.position.y) {
                var rg = GetComponent<Rigidbody2D>();
                rg.velocity = new Vector2(velocity.x, Mathf.Abs(velocity.y));
                Debug.Log("up");
            }
        }

    }
}
