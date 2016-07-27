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

    private Rigidbody2D rg2d;

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
	}

	// Update is called once per frame
	void Update () {
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

        transform.position = newPos;
    }

    /* public helpers */
    public float y {
        get {
            return transform.position.y;
        }
    }

    public float x {
        get {
            return transform.position.x;
        }
    }
}
