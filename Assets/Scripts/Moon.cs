using UnityEngine;
using System.Collections;

public class Moon : MonoBehaviour {

    public GameObject beam;
    public float pauseBetweenBeams = 5f;
    public float sweepDuration = 2f;
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 0.5f;
    public float sweepAngle = 25f;

    private Renderer beamR;

	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().sortingLayerName = "Midground";
        beamR = beam.GetComponent<Renderer>();
        StartCoroutine(Searchlight());
	}

    IEnumerator Searchlight() {
        SetBeamToAngle(-sweepAngle);
        bool sweepRight = true;
        while (true) {
            // fade in beam
            yield return StartCoroutine(FadeBeam(0, 1));

            // rotate beam
            if (sweepRight) {
                yield return StartCoroutine(RotateBeamToAngle(-1 * sweepAngle, sweepAngle));
            } else {
                yield return StartCoroutine(RotateBeamToAngle(sweepAngle, -1 * sweepAngle));
            }

            // fade out beam
            yield return StartCoroutine(FadeBeam(1, 0));

            sweepRight = !sweepRight;

            // rest
            yield return new WaitForSeconds(pauseBetweenBeams);
        }
    }

    void SetBeamToAngle(float angle) {
        var rot = transform.rotation;
        rot.eulerAngles = new Vector3(0, 0, angle);
        transform.rotation = rot;
    }

    IEnumerator FadeBeam(float from, float to) {
        var currentColor = beamR.material.color;
        currentColor.a = from;
        beamR.material.color = currentColor;
        var fadeInStart = Time.time;
        while (fadeInStart + fadeInDuration> Time.time) {
            var alpha = Mathf.Lerp(from, to, (Time.time - fadeInStart) / fadeInDuration);
            currentColor = beamR.material.color;
            currentColor.a = alpha;
            beamR.material.color = currentColor;
            yield return null;
        }
    }

    IEnumerator RotateBeamToAngle(float startAngle, float angle) {
        float sweepStart = Time.time;
        while (sweepDuration + sweepStart > Time.time) {
            float targetAngle = Mathf.Lerp(startAngle, angle, (Time.time - sweepStart) / sweepDuration);
            Debug.Log(targetAngle);
            SetBeamToAngle(targetAngle);
            yield return null;
        }
    }
}
