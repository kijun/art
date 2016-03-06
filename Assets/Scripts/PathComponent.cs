using UnityEngine;
using System.Collections;

[RequireComponent (typeof (EdgeCollider2D))]
public class PathComponent : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other) {
        // if bubble
        var bubble = other.GetComponent<Bubble>();
        if (bubble != null)  {
            var brgbd = bubble.GetComponent<Rigidbody2D>();
            Debug.Log(transform.rotation);
            brgbd.velocity = (transform.rotation * Vector3.up).normalized * pathController.speed;
            Debug.Log(brgbd.velocity);
            brgbd.angularVelocity = pathController.angularVelocity;
        }
    }

    PathController pathController {
        get {
            return transform.GetComponentInParent<PathController>();
        }
    }
}
