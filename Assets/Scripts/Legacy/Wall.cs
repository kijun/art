using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

    public Vector2 displacement = new Vector2(2, 0);
    public float lerpDuration = 3f;
    private Vector2 startPos;
    private Vector2 endPos;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
        endPos = transform.position + (Vector3)displacement;
        StartCoroutine(RunLerp());
	}

    IEnumerator RunLerp() {
        while (true) {
            float lerp = Mathf.PingPong(Time.time, lerpDuration) / lerpDuration;
            transform.position = Vector3.Lerp(startPos, endPos, lerp);
            yield return null;
        }
    }
}
