using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
public class Movement : MonoBehaviour {

    public Vector2 velocity;
    public float angularVelocity;

	// Use this for initialization
	void Start () {
        var rg = GetComponent<Rigidbody2D>();
        rg.velocity = velocity;
        rg.angularVelocity = angularVelocity;
	}
}
