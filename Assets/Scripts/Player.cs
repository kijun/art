using UnityEngine;
//using System;
using System.Collections;
using System.Collections.Generic;


public class Player : MonoBehaviour {

    public float rotationConst = 5f;
    public float maxYSpeed = 4f;
    public float maxXSpeed = 2f;
    public float xSpeed = 1f;
    public float ySpeed = 1f;
    public float thrust = 4f;
    public float maxAltitude = 15f;

    public int maxHealth = 100;
    public int damagePerHit = 30;
    public Count spinTorque = new Count(30, 100);
    public float spinDuration = 2f;
    public AudioSource soundSource;
    public AudioClip hitSound;

    public enum State {
        Start,
        Normal,
        Hit,
        Destroyed,
        Won
    }

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
                float xdir = Input.GetAxis("Horizontal");
                float ydir = Input.GetAxis("Vertical");
                // left, right
                if (Mathf.Abs(xdir) > float.Epsilon) {
                    xdir = Mathf.Sign(xdir);
                    if ((xdir > 0 & rg2d.velocity.x < xdir * maxXSpeed) ||
                        (xdir < 0 & rg2d.velocity.x > xdir * maxXSpeed)) {
                        // if different direction then reset x velocity
                        if (xdir != Mathf.Sign(rg2d.velocity.x)) {
                            rg2d.velocity = new Vector2(0, rg2d.velocity.y);
                        }
                        rg2d.AddForce(new Vector2(xdir*xSpeed, 0));
                    }
                } else {
                    rg2d.velocity = new Vector2(0, rg2d.velocity.y);
                }

                // up
                if (ydir > float.Epsilon) {
                    if (Mathf.Abs(rg2d.velocity.y) < maxYSpeed) {
                        rg2d.AddForce(new Vector2(0, ySpeed));
                    }
                    // animate
                }

                if (maxAltitude < altitude) {
                    Debug.Log("You win");
                    currentState = State.Won;
                }

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

    void OnTriggerEnter2D (Collider2D other) {
        /*
        Artifact art = other.GetComponent<Artifact>();
        if (art != null) {
            art.ShrinkAndHide();
            UIManager.instance.ShowText(art.poetry);
            inventory.Add(art);
        }
        */
        Artifact art = other.GetComponent<Artifact>();
        if (art != null) {
            Debug.Log("triggered");
            art.BlazeAndFade();
            UIManager.instance.ShowText(art);
        } else {
            Destroy(other.gameObject);
            OnHit();
        }
    }

    void OnCollisionEnter2D(Collision2D coll) {
        // if planet
        // play destroy animation
        /*foreach (var art in inventory) {
            art.transform.position = transform.position;
            art.transform.localScale = Vector3.one;
            art.GetComponent<Rigidbody2D>().isKinematic = false;
            art.GetComponent<Rigidbody2D>().AddForce(new Vector2(1,1), ForceMode2D.Impulse);
        }
        */
        //coll.gameObject; // what do i want it to do? i wanted it to destruct and spin the ship
        //inventory.Clear();
        OnHit();
        // end game
        //GameManager.instance.EndGame();
    }

    void OnHit() {
        health -= damagePerHit;
        Spin();
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
                Random.Range(spinTorque.minimum, spinTorque.maximum),
                ForceMode2D.Impulse);
    }

    float altitude {
        get {
            return transform.position.y;
        }
    }

    IEnumerator ShowHealthBar() {
        // set health
        // fade in healthbar
        yield return new WaitForSeconds(2);
        // fade out healthbar
        yield return null;
    }
}
