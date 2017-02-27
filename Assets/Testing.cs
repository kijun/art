using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Testing : MonoBehaviour {
    public float startTime = 0;
    public AudioSource mainAudioSource;
    bool fastForward;
	// Use this for initialization
	void Start () {
        if (startTime > float.Epsilon) {
            Time.timeScale = 100;

            fastForward = true;
            if (mainAudioSource != null) {
                mainAudioSource.Stop();
            }
        }
	}

	// Update is called once per frame
	void Update () {
        if (fastForward && Time.time > startTime) {
            Time.timeScale = 1;
            if (mainAudioSource != null) {
                mainAudioSource.time = Time.time;
                mainAudioSource.Play();
            }
            fastForward = false;
        }
	}
}
