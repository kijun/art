using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animatable : MonoBehaviour {

    Vector2 originalScale;
    float originalRotation;

    void Start() {
        originalScale = localScale;
        originalRotation = rotation;
    }

    void FixedUpdate() {
        if (Mathf.Abs(scaleVelocity.sqrMagnitude) > float.Epsilon) {
            transform.localScale += (Vector3)scaleVelocity * Time.deltaTime;
        }
    }

    /*** PUBLIC|METHODS ***/
    public void StopMovement() {
        velocity = Vector2.zero;
        angularVelocity = 0;
        scaleVelocity = Vector2.zero;
    }

    public bool stationary {
        get {
            return velocity.sqrMagnitude < float.Epsilon &&
                   Mathf.Abs(angularVelocity) < float.Epsilon &&
                   scaleVelocity.sqrMagnitude < float.Epsilon;
        }
    }
    /*** PUBLIC|ANIMATABLE PROPERTIES ***/

    public Vector2 position {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public float rotation {
        get { return rigidbody2D.rotation; }
        set { rigidbody2D.rotation = value; }
    }

    public Rigidbody2D rigidbody2D {
        get { return GetComponent<Rigidbody2D>(); }
    }

    public Vector2 velocity {
        get { return rigidbody2D.velocity; }
        set { rigidbody2D.velocity = value; }
    }

    public Vector2 localScale {
        get { return transform.localScale; }
        set { transform.localScale = new Vector3(value.x, value.y, transform.localScale.z); }
    }

    public float angularVelocity {
        get { return rigidbody2D.angularVelocity; }
        set { rigidbody2D.angularVelocity = value; }
    }

    public Vector2 scaleVelocity;

}
