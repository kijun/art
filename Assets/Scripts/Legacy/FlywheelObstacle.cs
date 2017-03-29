using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]

public class FlywheelObstacle : MonoBehaviour {

    public float angularVelocity = 100;

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody2D>().angularVelocity = angularVelocity;
	}
}
