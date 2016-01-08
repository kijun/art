﻿using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    public float rotationConst = 5f;

    private Rigidbody2D rg2d;
    private List<Artifact> inventory;

	// Use this for initialization
	void Awake () {
        rg2d = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void FixedUpdate () {

        if (!GameManager.instance.journeying) return;

        var inputDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //rg2d.rotation

        if (inputDir != Vector2.zero) {
            var radRotation = rg2d.rotation / 360f * 2 * Mathf.PI;
            var rot = new Vector2(Mathf.Sin(-1 * radRotation), Mathf.Cos(radRotation));

            float zDir = Vector3.Cross(rot, inputDir).z;
            float angle = Vector2.Angle(inputDir, rot);

            //Debug.Log("angle " + angle + " input " + inputDir + " rot " + rot + " zdir " + zDir);

            if (zDir < -1 * float.Epsilon) {
                Debug.Log("turn left");
                rg2d.AddTorque(-1f * rotationConst * Time.deltaTime);
            } else if (zDir > float.Epsilon) {
                Debug.Log("turn right");
                rg2d.AddTorque(rotationConst * Time.deltaTime);
            }

            rg2d.AddForce(inputDir);
        }
	}

    void OnTriggerEnter2D (Collider2D other) {
        Artifact art = other.GetComponent<Artifact>();
        if (art != null) {
            art.ShrinkAndHide();
        }
    }
}