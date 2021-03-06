using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public delegate void OnHitDelegate();

[System.Serializable]
public class ShipStats {
    public float maxXSpeed = 1.05f;
    public float maxYSpeed = 1.05f;
    public float baseYSpeed = 0f;
}

public class PlayerController : MonoBehaviour {

    //public float xSpeed { get; private set;}
    public float yDeltaSpeed;// {get; private set;}
    public float yBaseSpeed;// {get; private set;}
    public float stroke1BaseAngularVelocity;
    public float stroke1MaxAngularVelocity;
    public float stroke2BaseAngularVelocity;
    public float stroke2MaxAngularVelocity;
    public Rigidbody2D stroke1;
    public Rigidbody2D stroke2;

    public OnHitDelegate OnHit;


    private Vector2 originalPosition;
    private Rigidbody2D rg2d;
    //TODO remove
    private bool upOnce = true;

    // Stats
    public void LockCurrentRegion() {
        yBaseSpeed = 0;
    }

    public void UnlockCurrentRegion() {
    }

	// Use this for initialization
	void Awake () {
        rg2d = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
	}

	// Update is called once per frame
	void FixedUpdate () {
        float xdir = Input.GetAxisRaw("Horizontal");
        float ydir = Input.GetAxisRaw("Vertical");
        var v = new Vector2(xdir, ydir);
        var vmag = v.sqrMagnitude;
        if (vmag < float.Epsilon) {
            rg2d.AddForce(rg2d.velocity * -1f);
        } else {
            // if velocity is larger than 25, do not add
            // if velocity is zero, v * 10
            var f = v * 10 / (vmag + 1);
            rg2d.AddForce(f);
        }

        /*
        if (maxAltitude < altitude) {
            Debug.Log("You win");
            currentState = State.Won;
        }
        */

    }
    /*
    public void Reset() {
        //transform.position = originalPosition;
    }

    Vector2 ConstrainPoint(Vector2 point) {
        if (!localPositionConstraint.bounds.Contains(point)) {
            point = localPositionConstraint.bounds.ClosestPoint(point);
        }
        return point;
    }

    void OnTriggerEnter2D (Collider2D other) {
        if (!other.gameObject.tag.Equals(Tags.Bullet)) {
            return;
        }
        Respawn();

        //Debug.Log("hit by" + other + other.gameObject.name);
        //OnHit();
        //soundSource.PlayOneShot(hitSound);
        //OnHit();
    }

    void Respawn() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, new Vector3(0, 0, 1), out hit, 500, Consts.patternBackgroundLayerMask)) {
            ChangeState(State.Hit);
            var go = hit.collider.gameObject;
            StartCoroutine(FadeInOut(go));
        } else {
            return;
            Debug.LogError("raycast result null", this);
        }
    }

    IEnumerator FadeInOut(GameObject go) {
        fader.fadeIn = false;
        yield return new WaitForSeconds(2f);
        var pattern = go.transform.parent;
        transform.position = pattern.position;
        LockCurrentRegion();
        upOnce = true;
        CameraController.instance.ResetPosition();
        fader.fadeIn = true;
        ChangeState(State.Normal);
    }
    */
}

