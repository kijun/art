using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D), typeof (Collider2D))]
public class Bubble : MonoBehaviour {

    public bool canMove;
    public float activationDistance;
    public Vector2 dPos;

    private Vector2 prevPos;

    // assume this for now
    //public bool doesSetZoneVelocity = true;

    /* override methods */
	void Start () {
        prevPos = transform.position;
	}

	void Update () {
        dPos = (Vector2)transform.position - prevPos;
        prevPos = transform.position;

        if (canMove) return;

        if (activationDistance > float.Epsilon) {
            if (PlayerController.instance.y > this.transform.position.y - activationDistance) {
                canMove = true;
            }
        }
	}

    public void UpdatePlayerVelocity() {
        //PlayerController.instance.SetZoneVelocityAndMaxRelativeSpeed(
        var vel = GetComponent<Rigidbody2D>().velocity;
        /*
        if (vel == Vector2.zero) {
            PlayerController.instance.SetZoneVelocityAndMaxRelativeSpeedToDefault();
        } else {
        */
            PlayerController.instance.zoneVelocity = vel;
        //}
    }

    /*
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.IsPlayer()) {
            PlayerController.instance.zoneVelocity = GetComponent<Rigidbody2D>().velocity;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.IsPlayer()) {
            // what happens when there's another object?
            var bubbles = PlayerController.instance.FindOverlappingBubbles();
            if (bubbles.Count > 1) {
                // then... what?
            }
            PlayerController.instance.SetZoneVelocityAndMaxRelativeSpeedToDefault();
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.IsPlayer()) {
            PlayerController.instance.zoneVelocity = GetComponent<Rigidbody2D>().velocity;
        }
    }
    */

    /* public methods */
}
