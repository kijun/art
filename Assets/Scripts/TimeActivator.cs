using UnityEngine;
using System.Collections;

public class TimeActivator : MonoBehaviour {
    public float delay;
    public MonoBehaviour toActivate;
    private bool activated;

	// Use this for initialization
	void Start () {
        if (toActivate == null) {
            foreach (var m in GetComponents<MonoBehaviour>()) {
                if (m != this) {
                    toActivate = m;
                    break;
                }
            }
        }

        if (toActivate != null)  {
            toActivate.enabled = false;
        }

        StartCoroutine(ActivateWithWait(delay));
	}


    public void ActivateNow() {
        if (!activated) {
            toActivate.enabled = true;
            activated = true;
        }
    }

    IEnumerator ActivateWithWait(float d) {
        yield return new WaitForSeconds(d);
        ActivateNow();
    }
}
