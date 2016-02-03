using UnityEngine;
using System.Collections;

public class RevolvingDoor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody2D>().angularVelocity = 30;
	}

	// Update is called once per frame
	void Update () {
	}
}
