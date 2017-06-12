using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    public TimeSignature timeSignature;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
        var text = Time.time.ToString() + "\n";
        if (timeSignature.beatsPerMinute > float.Epsilon) {
            text += timeSignature.TimeToString(Time.time);
        }
        GetComponent<Text>().text = text;
	}
}
