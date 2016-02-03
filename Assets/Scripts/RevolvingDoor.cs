using UnityEngine;
using System.Collections;

public class RevolvingDoor : MonoBehaviour {

    public float angularVelocity = 30;

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody2D>().angularVelocity = angularVelocity;
	}

	// Update is called once per frame
	void Update () {
	}
}
