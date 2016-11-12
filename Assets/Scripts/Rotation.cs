using UnityEngine;
using System.Collections;

/*[RequireComponent (typeof (Rigidbody2D))]*/
public class Rotation : MonoBehaviour {

    public float angularVelocity = 30;

    bool manualRotation;

	// Use this for initialization
	void Start () {
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null) {
            rb.angularVelocity = angularVelocity;
        } else {
            manualRotation = true;
        }
	}


    void Update() {
        if (manualRotation) {
            transform.Rotate(new Vector3(0, 0, angularVelocity*Time.deltaTime));
        }
    }
}

