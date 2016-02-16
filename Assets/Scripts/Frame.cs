using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D), typeof (Rigidbody2D))]

public class Frame : MonoBehaviour {

    void Start() {
        foreach (Transform c in transform) {
            c.GetComponent<Renderer>().material.SetAlpha(0.2f);
        }
    }
}
