using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public delegate void OnHitDelegate();

[System.Serializable]
public class ShipStats {
    public float maxXSpeed = 1.05f;
    public float maxYSpeed = 1.05f;
    public float baseYSpeed = 0.5f;
}

public class PlayerController : MonoBehaviour {

    //public float xSpeed { get; private set;}
    public float yDeltaSpeed {get; private set;}
    public float yBaseSpeed {get; private set;}

    public ShipStats stats;

    public AudioSource soundSource;
    public AudioClip hitSound;
    public BoxCollider2D localPositionConstraint;

    public enum State {
        Start,
        Normal,
        Hit,
        Destroyed,
        Won
    }

    public OnHitDelegate OnHit;


    private Vector2 originalPosition;
    private State currentState = State.Start;
    private Rigidbody2D rg2d;
    //TODO remove
    private bool upOnce = true;

    // Stats
    public void LockCurrentRegion() {
        yDeltaSpeed = stats.maxYSpeed;
        yBaseSpeed = 0;
    }

    public void UnlockCurrentRegion() {
        yDeltaSpeed = stats.maxYSpeed - stats.baseYSpeed;
        yBaseSpeed = stats.baseYSpeed;
    }

	// Use this for initialization
	void Awake () {
        rg2d = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
	}

	// Update is called once per frame
	void Update () {
        switch (currentState) {
            case State.Start:
                //if (GameManager.instance.journeying) {
                    currentState = State.Normal;
                //}
                break;
            case State.Normal:
                if (upOnce) {
                    if (Input.GetKeyDown("up")) {
                        UnlockCurrentRegion();
                        upOnce = false;
                    }
                }
                float xdir = Input.GetAxisRaw("Horizontal");
                float ydir = Input.GetAxisRaw("Vertical");
                Vector2 newPos = transform.position;

                newPos.x += xdir * stats.maxXSpeed * Time.deltaTime;
                newPos.y += (ydir * yDeltaSpeed + yBaseSpeed) * Time.deltaTime;

                newPos = ConstrainPoint(newPos);

                transform.position = newPos;

                /*
                if (maxAltitude < altitude) {
                    Debug.Log("You win");
                    currentState = State.Won;
                }
                */

                break;
            case State.Hit:
                /*
                if (Time.time > spinUntil) {
                    currentState = State.Normal;
                }
                */
                break;
            case State.Destroyed:
                break;
            case State.Won:
                Debug.Log("you won!");
                break;
        }
    }

    public void ChangeState(State state) {
        currentState = state;
    }

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
        Debug.Log("hit by" + other + other.gameObject.name);
        if (!other.gameObject.tag.Equals(Tags.Bullet)) {
            return;
        }

        //Debug.Log("hit by" + other + other.gameObject.name);
        //OnHit();
        soundSource.PlayOneShot(hitSound);
        //OnHit();
    }
}
