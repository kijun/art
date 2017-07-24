using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class Player : MonoBehaviour {

    public float xSpeed = 1f;
    public float ySpeed = 1f;
    public float maxAltitude = 15f;

    public int maxHealth = 100;
    public int damagePerHit = 30;
    public Count spinTorque = new Count(30, 100);
    public float spinDuration = 2f;
    public AudioSource soundSource;
    public AudioClip hitSound;
    public BoxCollider2D localPositionConstraint;

    public float altitude = 0f;
    public float fixedYSpeed = 0f;

    public enum State {
        Start,
        Normal,
        Hit,
        Destroyed,
        Won
    }

    public delegate void OnHitDelegate();

    private float spinUntil;
    private State currentState = State.Start;
    private int health;
    private Rigidbody2D rg2d;
    private List<Artifact> inventory = new List<Artifact>();

	// Use this for initialization
	void Awake () {
        rg2d = GetComponent<Rigidbody2D>();
        health = maxHealth;
	}

	// Update is called once per frame
	void FixedUpdate () {
        switch (currentState) {
            case State.Start:
                if (GameManager.instance.journeying) {
                    currentState = State.Normal;
                }
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
                if (Time.time > spinUntil) {
                    currentState = State.Normal;
                }
                break;
            case State.Destroyed:
                break;
            case State.Won:
                Debug.Log("you won!");
                break;
        }
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
        OnHit();
    }

    void OnCollisionEnter2D(Collision2D coll) {
        return;
        OnHit();
    }

    void OnHit() {
        health -= damagePerHit;
        //Spin();
        StartCoroutine(ShowHealthBar());
        if (health < 0)  {
            currentState = State.Destroyed;
        } else {
            currentState = State.Hit;
        }
        spinUntil = Time.time + spinDuration;
        CameraFollow.instance.ScreenShake();
        soundSource.PlayOneShot(hitSound);
    }

    void Spin() {
        rg2d.AddTorque(
                UnityEngine.Random.Range(spinTorque.minimum, spinTorque.maximum),
                ForceMode2D.Impulse);
    }

    IEnumerator ShowHealthBar() {
        // set health
        // fade in healthbar
        yield return new WaitForSeconds(2);
        // fade out healthbar
        yield return null;
    }
}
