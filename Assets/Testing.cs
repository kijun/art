using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Testing : MonoBehaviour {
    public float startTime = 0;
    public float timeScale = 1;
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
        } else {
            Time.timeScale = timeScale;
        }
	}

	// Update is called once per frame
	void Update () {
        if (fastForward && Time.time > startTime) {
            Time.timeScale = timeScale;
            if (mainAudioSource != null) {
                mainAudioSource.time = Time.time / timeScale;
                mainAudioSource.Play();
            }
            fastForward = false;
        }
	}
}
