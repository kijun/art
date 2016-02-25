using UnityEngine;
using System.Collections;

public class PatternLauncher : MonoBehaviour {

    public GameObject toActivateInPlayMode;
    bool launched = false;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
        // we put this after the start function as Activator might reawake and reinitialize
        if (!launched) {
            if (toActivateInPlayMode != null) {
                var activator = toActivateInPlayMode.GetComponent<TimeActivator>();
                activator.ActivateNow();
            }
            launched = true;
        }
	}
}
