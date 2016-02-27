using UnityEngine;
using System.Collections;


public delegate void OnBecameInvisibleDelegate(GameObject go);

public class BackgroundObject : MonoBehaviour {

    public OnBecameInvisibleDelegate onBecameInvisibleDelegate;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

    void OnBecameInvisible() {
        Debug.Log("AAA INVIS");
        onBecameInvisibleDelegate(gameObject);
    }
}
