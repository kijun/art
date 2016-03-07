using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D), typeof (Collider2D))]
public class Bubble : MonoBehaviour {

    // assume this for now
    //public bool doesSetZoneVelocity = true;

    /* override methods */
	void Start () {

	}

	void Update () {

	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.IsPlayer()) {
            PlayerController.instance.zoneVelocity = GetComponent<Rigidbody2D>().velocity;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.IsPlayer()) {
            PlayerController.instance.SetZoneVelocityAndMaxRelativeSpeedToDefault();
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.IsPlayer()) {
            PlayerController.instance.zoneVelocity = GetComponent<Rigidbody2D>().velocity;
        }
    }

    /* public methods */
}
