using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DummyPlayerController : MonoBehaviour {

    //public float xSpeed { get; private set;}
    public float yDeltaSpeed {get; private set;}
    public float yBaseSpeed {get; private set;}
    public float stroke1BaseAngularVelocity;
    public float stroke1MaxAngularVelocity;
    public float stroke2BaseAngularVelocity;
    public float stroke2MaxAngularVelocity;
    public Rigidbody2D stroke1;
    public Rigidbody2D stroke2;

    public AudioSource soundSource;
    public AudioClip hitSound;
    public BoxCollider2D localPositionConstraint;
    public ScreenFader fader;

    public enum State {
        Start,
        Normal,
        Hit,
        Destroyed,
        Won
    }


    private Vector2 originalPosition;
    private State currentState = State.Start;
    private Rigidbody2D rg2d;
    //TODO remove
    private bool upOnce = true;

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
                float xdir = Input.GetAxisRaw("Horizontal");
                float ydir = Input.GetAxisRaw("Vertical");

                    stroke1.angularVelocity = stroke1BaseAngularVelocity;
                    stroke2.angularVelocity = stroke2BaseAngularVelocity;


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
}
