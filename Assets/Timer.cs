using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    public TimeSignature timeSignature;
    float startTime;

	// Use this for initialization
	void Start () {
        startTime = Time.time;
	}

	// Update is called once per frame
	void Update () {
        var text = (Time.time - startTime).ToString() + "\n";
        if (timeSignature.beatsPerMinute > float.Epsilon) {
            text += timeSignature.TimeToString(Time.time-startTime);
        }
        GetComponent<Text>().text = text;
	}
}
