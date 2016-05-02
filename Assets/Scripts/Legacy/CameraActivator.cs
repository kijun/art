using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
public class CameraActivator : MonoBehaviour {

    public MonoBehaviour toActivate;
    public bool destructOnExit;

    void Start() {
        toActivate.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.IsCamera()) {
            toActivate.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (destructOnExit && other.gameObject.IsCamera()) {
            Destroy(this.gameObject);
        }
    }
}
