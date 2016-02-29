using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
public class Rotation : MonoBehaviour {

    public float angularVelocity = 30;

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody2D>().angularVelocity = angularVelocity;
	}
}

