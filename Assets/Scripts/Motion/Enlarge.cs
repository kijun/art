using UnityEngine;
using System.Collections;

public class Enlarge : MonoBehaviour {


    /***** PUBLIC: VARIABLES *****/
    public float enlargeDuration;
    public float scaleMultiplier = 1;
    public bool repeat = true;


    /***** INITIALIZER *****/
	// Use this for initialization
	void Start () {
        StartCoroutine(RunEnlarge());
	}


    /***** PRIVATE: METHODS *****/
    IEnumerator RunEnlarge() {
        Vector2 startScale = transform.localScale;
        Vector2 endScale = startScale * scaleMultiplier;

        while (true) {
            var startTime = Time.time;
            while (Time.time < startTime + enlargeDuration) {
                transform.localScale = startScale + ((endScale - startScale) / enlargeDuration) * (Time.time - startTime);
                yield return null;
            }

            if (!repeat) { break; }

            startTime = Time.time;
            while (Time.time < startTime + enlargeDuration) {
                transform.localScale = endScale - ((endScale - startScale) / enlargeDuration) * (Time.time - startTime);
                yield return null;
            }
        }
    }

}
