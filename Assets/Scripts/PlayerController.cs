using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public delegate void OnHitDelegate();

public class PlayerController : MonoBehaviour {

    public float xSpeed = 1f;
    public float ySpeed = 1f;

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


    private State currentState = State.Start;
    private Rigidbody2D rg2d;

	// Use this for initialization
	void Awake () {
        rg2d = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void FixedUpdate () {
        switch (currentState) {
            case State.Start:
                //if (GameManager.instance.journeying) {
                    currentState = State.Normal;
                //}
                break;
            case State.Normal:
                float xdir = Input.GetAxisRaw("Horizontal");
                float ydir = Input.GetAxisRaw("Vertical");
                Vector2 newPos = transform.position;

                newPos.x += xdir * xSpeed * Time.deltaTime;
                newPos.y += ydir * ySpeed * Time.deltaTime;

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
        //OnHit();
        soundSource.PlayOneShot(hitSound);
        OnHit();
    }
}
