using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public delegate void OnHitDelegate();

public class PlayerController : MonoBehaviour {

    public static PlayerController instance;

    /* basic stats */
    public Vector2 zoneVelocity = Consts.defaultZoneBaseVelocity;
    public Vector2 maxRelativeSpeed = Consts.defaultZoneMaxRelativeSpeed;

    /* stroke movement - only visual */
    public float stroke1BaseAngularVelocity;
    public float stroke1MaxAngularVelocity;
    public float stroke2BaseAngularVelocity;
    public float stroke2MaxAngularVelocity;
    public Rigidbody2D stroke1;
    public Rigidbody2D stroke2;

    /* random components */
    public AudioSource soundSource;
    public AudioClip hitSound;
    public BoxCollider2D localPositionConstraint;
    public ScreenFader fader;
    public OnHitDelegate OnHit;

    public enum State {
        Start,
        Normal,
        Hit,
        Destroyed,
        Won
    }

    /* private states and references */
    private State currentState = State.Start;
    private Rigidbody2D rg2d;
    //TODO remove
    private bool frozen = true;

	// Use this for initialization
	void Awake () {
        rg2d = GetComponent<Rigidbody2D>();
        // Assign static instance
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        Freeze();
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
                if (frozen) {
                    if (Input.GetKeyDown("up")) {
                        // idea of current zone
                        SetZoneVelocityAndMaxRelativeSpeedToDefault();
                        frozen = false;
                    }
                }
                float xdir = Input.GetAxisRaw("Horizontal");
                float ydir = Input.GetAxisRaw("Vertical");
                Vector2 newPos = transform.position;

                float dx = (xdir * maxRelativeSpeed.x + zoneVelocity.x) * Time.deltaTime;
                float dy = (ydir * maxRelativeSpeed.y + zoneVelocity.y) * Time.deltaTime;

                newPos.x += dx;
                newPos.y += dy;

                if (Mathf.Abs(xdir) + Mathf.Abs(ydir) > float.Epsilon) {
                    stroke1.angularVelocity = stroke1MaxAngularVelocity;
                    stroke2.angularVelocity = stroke2MaxAngularVelocity;
                } else {
                    stroke1.angularVelocity = stroke1BaseAngularVelocity;
                    stroke2.angularVelocity = stroke2BaseAngularVelocity;
                }

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

    /** velocity and speed setters **/

    public void SetZoneVelocityAndMaxRelativeSpeed(Vector2 zv, Vector2 rs) {
        zoneVelocity = zv;
        maxRelativeSpeed = rs;
    }

    public void SetZoneVelocityAndMaxRelativeSpeedToDefault() {
        Debug.Log("AAA");
        var z = ZoneController.ZoneForPosition(transform.position);
        zoneVelocity = z.zoneBaseVelocity;
        maxRelativeSpeed = z.maxRelativeSpeed;
    }

    public void Freeze() {
        SetZoneVelocityAndMaxRelativeSpeed(Vector2.zero, Vector2.zero);
    }

    public void Freeze(out Vector2 currentZoneVelocity, out Vector2 currentMaxRelativeSpeed) {
        currentZoneVelocity = zoneVelocity;
        currentMaxRelativeSpeed = maxRelativeSpeed;
        Freeze();
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

    /** respawn && bubble entrance **/
    List<Bubble> bubbles = new List<Bubble>();

    void OnTriggerExit2D (Collider2D other) {
        var bubble = other.GetComponent<Bubble>();
        if (bubble != null) {
            bubbles.Remove(bubble);
            if (bubbles.Count == 0) Respawn();
        }
    }

    void OnTriggerStay2D (Collider2D other) {
        var bubble = other.GetComponent<Bubble>();
        if (bubble != null) {
            if (bubbles[0] == bubble) {
                if (!frozen) bubble.UpdatePlayerVelocity();
            }
        }
    }

    void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.tag.Equals(Tags.Bullet)) {
            Debug.Log("hit by" + other + other.gameObject.name);
            Respawn();
            //OnHit();
            //soundSource.PlayOneShot(hitSound);
            //OnHit();
        } else {
            var bubble = other.GetComponent<Bubble>();
            if (bubble != null && !bubbles.Contains(bubble)) {
                    bubbles.Add(bubble);
            }
        }
    }

    public void Respawn() {
        ZoneController zone = ZoneController.ZoneForPosition(transform.position);
        if (zone != null) {
            StartCoroutine(FadeInOut(zone));
        } else {
            Debug.LogError("raycast result null", this);
        }
    }

    IEnumerator FadeInOut(ZoneController zone) {
        fader.fadeIn = false;
        Vector2 zoneVCache, relSpeedCache;
        Freeze(out zoneVCache, out relSpeedCache);

        yield return new WaitForSeconds(2f);
        transform.position = zone.transform.position;
        frozen = true;
        CameraController.instance.ResetPosition();
        fader.fadeIn = true;
        ChangeState(State.Normal);
    }
}
