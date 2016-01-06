using UnityEngine;
using System.Collections.Generic;

public class SpaceshipController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //transform.forward = new Vector3(0, 1, 0);
	}

	// Update is called once per frame
	void Update () {
        var target = Input.mousePosition;
        target = Camera.main.ScreenToWorldPoint(target);

        //transform.rotation.SetLookRotation(Vector3.forward, new Vector3(1, 0, 0));
        if (Input.GetMouseButtonDown(0)) {
            GetComponent<Rigidbody2D>().AddForceAtPosition(
                    transform.position-target,
                    transform.position.IncrY(0.05f), // TODO use separate obj
                    ForceMode2D.Impulse);
        }
	}
}

public class Artifact {
}

public class StarField {
}
