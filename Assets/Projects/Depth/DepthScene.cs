using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        foreach (var d in Display.displays) {
            d.Activate();
        }
	}

	// Update is called once per frame
	void Update () {

	}
}
