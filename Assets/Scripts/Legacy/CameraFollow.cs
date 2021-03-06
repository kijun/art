﻿using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public static CameraFollow instance;

    public float xMargin = 0f;		// Distance in the x axis the player can move before the camera follows.
	public float yMargin = 0f;		// Distance in the y axis the player can move before the camera follows.
	public float xSmooth = 1f;		// How smoothly the camera catches up with it's target movement in the x axis.
	public float ySmooth = 1f;		// How smoothly the camera catches up with it's target movement in the y axis.

    private Transform player;
    private bool shaking;

	// Use this for initialization
	void Start () {
        shaking = false;
	}

    void Awake () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Assign static instance
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
//        DontDestroyOnLoad(gameObject);
    }

	bool CheckXMargin() {
		// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
		return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
	}


	bool CheckYMargin() {
		// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
		return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
	}

	// Update is called once per frame
	void FixedUpdate () {
        if (!shaking) {
            TrackPlayer();
        }
	}

    void TrackPlayer() {
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		float targetX = transform.position.x;
		float targetY = transform.position.y;

		// If the player has moved beyond the x margin...
		//if(CheckXMargin())
			// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
        //    targetX = player.position.x;
			//targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);

		// If the player has moved beyond the y margin...
		if(CheckYMargin())
			// ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
            targetY = player.position.y - yMargin;
			//targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);


		// Set the camera's position to the target position with the same z component.
		transform.position = new Vector3(targetX, targetY, transform.position.z);
    }

    public void ScreenShake() {
        shaking = true;
        InvokeRepeating("CameraShake", 0, .01f);
        Invoke("StopShaking", 0.3f);
    }

    //void OnCollisionEnter2D(Collision2D coll)
    //{

    //    shakeAmt = coll.relativeVelocity.magnitude * .0025f;

    //}

    void CameraShake() {
        float quakeAmt = Random.value*0.01f*2 - 0.01f;
        Vector3 pp = transform.position;
        pp.y+= quakeAmt; // can also add to x and/or z
        transform.position = pp;
    }

    void StopShaking() {
        shaking = false;
        CancelInvoke("CameraShake");
    }
}
